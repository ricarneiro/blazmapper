using BlazMapper;

Console.WriteLine("🚀 BlazMapper - Exemplos de Uso");
Console.WriteLine("================================\n");

// Exemplo 1: Mapeamento Básico com Classes
Console.WriteLine("📋 Exemplo 1: Mapeamento Básico");
Console.WriteLine("-------------------------------");

var sourcePerson = new SourcePerson
{
    Name = "João Silva",
    Age = 30,
    Email = "joao@email.com"
};

var destinationPerson = sourcePerson.MapTo<SourcePerson, DestinationPerson>();

Console.WriteLine($"Original: {sourcePerson.Name}, {sourcePerson.Age}, {sourcePerson.Email}");
Console.WriteLine($"Mapeado:  {destinationPerson.Name}, {destinationPerson.Age}, {destinationPerson.Email}");
Console.WriteLine();

// Exemplo 2: Mapeamento com Records (Objetos Imutáveis)
Console.WriteLine("🔒 Exemplo 2: Mapeamento com Records");
Console.WriteLine("-----------------------------------");

var sourceRecord = new SourceRecord("Maria", "Santos", 25, "Desenvolvedora");
var destinationRecord = sourceRecord.MapTo<SourceRecord, DestinationRecord>();

Console.WriteLine($"Original: {sourceRecord.FirstName} {sourceRecord.LastName}, {sourceRecord.Age}, {sourceRecord.Job}");
Console.WriteLine($"Mapeado:  {destinationRecord.FirstName} {destinationRecord.LastName}, {destinationRecord.Age}, {destinationRecord.Job}");
Console.WriteLine();

// Exemplo 3: Mapeamento de Objetos Complexos (Aninhados)
Console.WriteLine("🏢 Exemplo 3: Objetos Complexos");
Console.WriteLine("------------------------------");

var complexSource = new PersonWithAddress
{
    Name = "Pedro Costa",
    Age = 35,
    Address = new Address
    {
        Street = "Rua das Flores, 123",
        City = "São Paulo",
        ZipCode = "01234-567"
    }
};

var complexDestination = complexSource.MapTo<PersonWithAddress, PersonWithAddressDto>();

Console.WriteLine($"Original: {complexSource.Name}, {complexSource.Age}");
Console.WriteLine($"Endereço: {complexSource.Address.Street}, {complexSource.Address.City}");
Console.WriteLine($"Mapeado:  {complexDestination.Name}, {complexDestination.Age}");
Console.WriteLine($"Endereço: {complexDestination.Address.Street}, {complexDestination.Address.City}");
Console.WriteLine();

// Exemplo 4: Mapeamento com Tipos Anônimos
Console.WriteLine("🎭 Exemplo 4: Tipos Anônimos");
Console.WriteLine("---------------------------");

var anonymous = new { Name = "Ana Lima", Age = 28, Department = "TI" };
var fromAnonymous = anonymous.MapTo<object, Employee>();

Console.WriteLine($"Anônimo: Name={anonymous.Name}, Age={anonymous.Age}, Department={anonymous.Department}");
Console.WriteLine($"Mapeado: {fromAnonymous.Name}, {fromAnonymous.Age}, {fromAnonymous.Department}");
Console.WriteLine();

// Exemplo 5: Record Imutável com Construtor
Console.WriteLine("🎯 Exemplo 5: Record com Construtor Complexo");
Console.WriteLine("-------------------------------------------");

var productSource = new ProductSource
{
    Name = "Smartphone",
    Price = 1299.99m,
    Category = "Eletrônicos"
};

var productRecord = productSource.MapTo<ProductSource, ProductRecord>();

Console.WriteLine($"Original: {productSource.Name}, R$ {productSource.Price:F2}, {productSource.Category}");
Console.WriteLine($"Record:   {productRecord.Name}, R$ {productRecord.Price:F2}, {productRecord.Category}");
Console.WriteLine();

Console.WriteLine("✅ Todos os exemplos executados com sucesso!");
Console.WriteLine("\n💡 Pressione qualquer tecla para sair...");
Console.ReadKey();

// ==================== CLASSES DE EXEMPLO ====================

// Exemplo 1: Classes básicas
public class SourcePerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
}

public class DestinationPerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
}

// Exemplo 2: Records
public record SourceRecord(string FirstName, string LastName, int Age, string Job);
public record DestinationRecord(string FirstName, string LastName, int Age, string Job);

// Exemplo 3: Objetos complexos
public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class PersonWithAddress
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public Address Address { get; set; } = new();
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class PersonWithAddressDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public AddressDto Address { get; set; } = new();
}

// Exemplo 4: Employee
public class Employee
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
}

// Exemplo 5: Product
public class ProductSource
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}

public record ProductRecord(string Name, decimal Price, string Category);

