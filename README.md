# BlazMapper
----

## Description
Simple and efficient object mapper for .NET

## Descrição
Mapeador de objetos simples e eficiente para .NET*

----

## BlazMapper

(Português)
Clique no seu idioma preferido abaixo.
O triângulo (acima do idioma) exibe/esconde o texto.

(English)
Click on your preferred language below:
The triangle (above the language) shows/hides the text.

[![English](https://img.shields.io/badge/Language-English-blue)](#english) [![Português](https://img.shields.io/badge/Idioma-Português-green)](#portugues) [![NuGet](https://img.shields.io/nuget/v/BlazMapper.svg)](https://www.nuget.org/packages/BlazMapper) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

<details open>
<summary><h2 id="english">🇺🇸 English</h2></summary>

A simple and efficient library for object mapping in .NET, supporting both mutable and immutable objects with parameterized constructors.

## 🚀 Features
- **Automatic Mapping**: Maps properties automatically based on names (case-insensitive)
- **Immutable Objects**: Full support for objects with parameterized constructors
- **Mutable Objects**: Support for objects with settable properties
- **Implicit Conversions**: Detects and applies implicit conversions automatically
- **Recursive Mapping**: Maps complex nested objects
- **High Performance**: Uses reflection in an optimized way

## 📦 Installation
```bash
dotnet add package BlazMapper
```

## 🔧 How to Use

### Basic Usage
```csharp
using BlazMapper;

// For mutable objects
var source = new { Name = "John", Age = 30 };
var destination = source.MapTo<object, Person>();

// For immutable objects
public record PersonRecord(string Name, int Age);
var record = source.MapTo<object, PersonRecord>();
```

### Practical Examples

#### Simple Class Mapping
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
    Name = "Mary", 
    Age = 25, 
    Email = "mary@email.com" 
};

var destination = source.MapTo<SourcePerson, DestinationPerson>();
```

#### Mapping with Records (Immutable Objects)
```csharp
public record SourceRecord(string FirstName, string LastName, int Age);
public record DestinationRecord(string FirstName, string LastName, int Age);

var source = new SourceRecord("John", "Smith", 30);
var destination = source.MapTo<SourceRecord, DestinationRecord>();
```

#### Complex Object Mapping
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

// BlazMapper automatically maps nested objects
var source = new Person 
{ 
    Name = "Peter", 
    Address = new Address { Street = "Main St", City = "New York" }
};

var destination = source.MapTo<Person, PersonDto>();
```

## 🔍 How It Works

BlazMapper automatically analyzes source and destination types:

1. **For objects with parameterized constructors**: Maps properties to constructor parameters
2. **For mutable objects**: Creates instance and sets properties
3. **Conversions**: Applies implicit conversions when necessary
4. **Recursive mapping**: Maps complex objects automatically

## ⚡ Performance
- Uses optimized reflection
- Internal cache for better performance on repeated mappings
- Support for nested objects without performance loss

## 🤝 Contributing
Contributions are welcome! Feel free to:
- Report bugs
- Suggest improvements
- Submit pull requests

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links
- [GitHub Repository](https://github.com/yourusername/BlazMapper)
- [NuGet Package](https://www.nuget.org/packages/BlazMapper)

</details>

---

<details>
<summary><h2 id="portugues">🇧🇷 Português</h2></summary>

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

</details>

---

<p align="center">
  <strong>Desenvolvido com ❤️ para a comunidade .NET<br>
  Developed with ❤️ for the .NET community</strong>
</p>