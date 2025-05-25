using System.Diagnostics;
using System.Reflection;

namespace BlazMapper
{
    public static class BlazMapper
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TDestination : class
        {
            if (source == null)
                return default;

            var destType = typeof(TDestination);

            // Verificar se existe construtor público
            var constructors = destType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                      .OrderByDescending(c => c.GetParameters().Length)
                                      .ToList();

            // Se só houver um construtor com parametros, mapaer atribuito -> parametro
            if (constructors.Any() && constructors[0].GetParameters().Length > 0)
            {
                return MapToImmutableObject<TSource, TDestination>(source, constructors);
            }
            // Senão, usar mapeamento para objetos mutáveis
            else
            {
                return MapToMutableObject<TSource, TDestination>(source);
            }
        }

        private static TDestination MapToImmutableObject<TSource, TDestination>(TSource source, List<ConstructorInfo> constructors)
            where TDestination : class
        {
            var sourceType = typeof(TSource);
            var sourceProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Tenta todos os contrutores (pegar o mais completo?)
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    // Construtor sem parâmetros (apenas se errei)
                    var instance = constructor.Invoke(null);
                    return (TDestination)instance;
                }

                var parameterValues = new object[parameters.Length];
                bool canUseConstructor = true;

                // Mapear os parâmetros 
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];

                    var matchingProp = sourceProps.FirstOrDefault(p =>
                        string.Equals(p.Name, param.Name, StringComparison.OrdinalIgnoreCase));

                    if (matchingProp != null)
                    {
                        var value = matchingProp.GetValue(source);

                        if (param.ParameterType.IsAssignableFrom(matchingProp.PropertyType))
                        {
                            parameterValues[i] = value;
                        }
                        else if (value != null && TryImplicitConversion(value, param.ParameterType, out var convertedValue))
                        {
                            parameterValues[i] = convertedValue;
                        }
                        else if (value != null && !matchingProp.PropertyType.IsPrimitive &&
                                !matchingProp.PropertyType.Namespace.StartsWith("System"))
                        {
                            try
                            {
                                var method = typeof(BlazMapper).GetMethod(nameof(MapTo));
                                var genericMethod = method.MakeGenericMethod(matchingProp.PropertyType, param.ParameterType);
                                parameterValues[i] = genericMethod.Invoke(null, new[] { value });
                            }
                            catch
                            {
                                canUseConstructor = false;
                                break;
                            }
                        }
                        else
                        {
                            canUseConstructor = false;
                            break;
                        }
                    }
                    else
                    {
                        if (param.IsOptional)
                        {
                            parameterValues[i] = param.DefaultValue;
                        }
                        else
                        {
                            canUseConstructor = false;
                            break;
                        }
                    }
                }

                if (canUseConstructor)
                {
                    try
                    {
                        var instance = constructor.Invoke(parameterValues);
                        return (TDestination)instance;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            throw new InvalidOperationException(
                $"Não foi possível mapear {sourceType.Name} para {typeof(TDestination).Name} " +
                $"usando os construtores disponíveis. Verifique se os nomes das propriedades " +
                $"correspondem aos nomes dos parâmetros do construtor (case insensitive).");
        }

        private static TDestination MapToMutableObject<TSource, TDestination>(TSource source)
            where TDestination : class
        {
            var destType = typeof(TDestination);

            TDestination destination;
            try
            {
                destination = Activator.CreateInstance<TDestination>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar uma instância de {destType.Name}. " +
                    $"Certifique-se de que a classe tenha um construtor sem parâmetros acessível.", ex);
            }

            var sourceType = typeof(TSource);
            var sourceProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProps = destType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            foreach (var destProp in destProps)
            {
                if (!destProp.CanWrite)
                    continue;

                var sourceProp = sourceProps.FirstOrDefault(p =>
                    string.Equals(p.Name, destProp.Name, StringComparison.OrdinalIgnoreCase));

                if (sourceProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    if (value != null)
                    {
                        if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                        {
                            destProp.SetValue(destination, value);
                        }
                        else if (TryImplicitConversion(value, destProp.PropertyType, out var convertedValue))
                        {
                            destProp.SetValue(destination, convertedValue);
                        }
                        else if (!sourceProp.PropertyType.IsPrimitive &&
                                !sourceProp.PropertyType.Namespace.StartsWith("System"))
                        {
                            try
                            {
                                var method = typeof(BlazMapper).GetMethod(nameof(MapTo));
                                var genericMethod = method.MakeGenericMethod(sourceProp.PropertyType, destProp.PropertyType);
                                var mappedValue = genericMethod.Invoke(null, new[] { value });
                                destProp.SetValue(destination, mappedValue);
                            }
                            catch
                            {
                                Debug.WriteLine("Falha não prevista. Verificar como tratar.");
                            }
                        }
                    }
                }
            }

            return destination;
        }

        private static bool TryImplicitConversion(object source, Type destinationType, out object result)
        {
            result = null;
            if (source == null) return false;

            var sourceType = source.GetType();

            var methodSource = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == "op_Implicit" &&
                    m.ReturnType == destinationType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == sourceType);

            if (methodSource != null)
            {
                result = methodSource.Invoke(null, new[] { source });
                return true;
            }

            var methodDest = destinationType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == "op_Implicit" &&
                    m.ReturnType == destinationType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == sourceType);

            if (methodDest != null)
            {
                result = methodDest.Invoke(null, new[] { source });
                return true;
            }

            try
            {
                if (destinationType.IsValueType || destinationType == typeof(string))
                {
                    result = Convert.ChangeType(source, destinationType);
                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}
