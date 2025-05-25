# BlazMapper

Uma biblioteca simples e eficiente para mapeamento de objetos em .NET, suportando tanto objetos mutáveis quanto imutáveis com construtor parametrizado.

## 🚀 Características

- **Mapeamento Automático**: Mapeia propriedades automaticamente baseado nos nomes (case-insensitive)
- **Objetos Imutáveis**: Suporte completo para objetos com construtores parametrizados
- **Objetos Mutáveis**: Suporte para objetos com propriedades setáveis
- **Conversões Implícitas**: Detecta e aplica conversões implícitas automaticamente
- **Mapeamento Recursivo**: Mapeia objetos complexos aninhados
- **Alta Performance**: Usa reflection de forma otimizada

## 📦 Instalação

```bash
dotnet add package BlazMapper
```

## 🔧 Como Usar

### Uso Básico

```csharp
using BlazMapper;

// Para objetos mutáveis
var source = new { Name = "João", Age = 30 };
var destination = source.MapTo<object, Person>();

// Para objetos imutáveis
public record PersonRecord(string Name, int Age);
var record = source.MapTo<object, PersonRecord>();
```

### Exemplos Práticos

#### Mapeamento de Classes Simples

```csharp
public class SourcePerson
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

public class DestinationPerson
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

var source = new SourcePerson 
{ 
    Name = "Maria", 
    Age = 25, 
    Email = "maria@email.com" 
};

var destination = source.MapTo<SourcePerson, DestinationPerson>();
```

#### Mapeamento com Records (Objetos Imutáveis)

```csharp
public record SourceRecord(string FirstName, string LastName, int Age);
public record DestinationRecord(string FirstName, string LastName, int Age);

var source = new SourceRecord("João", "Silva", 30);
var destination = source.MapTo<SourceRecord, DestinationRecord>();
```

#### Mapeamento de Objetos Complexos

```csharp
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

public class Person
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

// O BlazMapper mapeia automaticamente objetos aninhados
var source = new Person 
{ 
    Name = "Pedro", 
    Address = new Address { Street = "Rua A", City = "São Paulo" }
};

var destination = source.MapTo<Person, PersonDto>();
```

## 🔍 Como Funciona

O BlazMapper analisa automaticamente os tipos de origem e destino:

1. **Para objetos com construtores parametrizados**: Mapeia propriedades para parâmetros do construtor
2. **Para objetos mutáveis**: Cria instância e define propriedades
3. **Conversões**: Aplica conversões implícitas quando necessário
4. **Mapeamento recursivo**: Mapeia objetos complexos automaticamente

## ⚡ Performance

- Usa reflection otimizada
- Cache interno para melhor performance em mapeamentos repetidos
- Suporte para objetos aninhados sem perda de performance

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:

- Reportar bugs
- Sugerir melhorias
- Enviar pull requests

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 🔗 Links

- [Repositório GitHub](https://github.com/seuusuario/BlazMapper)
- [Pacote NuGet](https://www.nuget.org/packages/BlazMapper)

---

Desenvolvido com ❤️ para a comunidade .NET