using BlazMapper.DomainSample.ValueObjects;
using Xunit;

namespace BlazMapper.Tests
{
    public class BlazMapperTests
    {
        #region Basic Mapping Tests

        [Fact]
        public void MapTo_BasicClasses_ShouldMapCorrectly()
        {
            // Arrange
            var source = new SourcePerson
            {
                Name = "João Silva",
                Age = 30,
                Email = "joao@email.com"
            };

            // Act
            var destination = source.MapTo<SourcePerson, DestinationPerson>();

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(source.Name, destination.Name);
            Assert.Equal(source.Age, destination.Age);
            Assert.Equal(source.Email, destination.Email);
        }

        [Fact]
        public void MapTo_NullSource_ShouldReturnNull()
        {
            // Arrange
            SourcePerson? source = null;

            // Act
            var destination = source.MapTo<SourcePerson, DestinationPerson>();

            // Assert
            Assert.Null(destination);
        }

        #endregion

        #region Record Mapping Tests

        [Fact]
        public void MapTo_Records_ShouldMapCorrectly()
        {
            // Arrange
            var sourceRecord = new SourceRecord("Maria", "Santos", 25, "Desenvolvedora");

            // Act
            var destinationRecord = sourceRecord.MapTo<SourceRecord, DestinationRecord>();

            // Assert
            Assert.NotNull(destinationRecord);
            Assert.Equal(sourceRecord.FirstName, destinationRecord.FirstName);
            Assert.Equal(sourceRecord.LastName, destinationRecord.LastName);
            Assert.Equal(sourceRecord.Age, destinationRecord.Age);
            Assert.Equal(sourceRecord.Job, destinationRecord.Job);
        }

        [Fact]
        public void MapTo_RecordWithDifferentParameterOrder_ShouldMapCorrectly()
        {
            // Arrange
            var source = new OrderedRecord("João", 30, "Gerente");

            // Act
            var destination = source.MapTo<OrderedRecord, ReorderedRecord>();

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(source.Name, destination.Name);
            Assert.Equal(source.Age, destination.Age);
            Assert.Equal(source.Position, destination.Position);
        }

        #endregion

        #region Complex Object Mapping Tests

        [Fact]
        public void MapTo_ComplexObjects_ShouldMapNestedProperties()
        {
            // Arrange
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

            // Act
            var complexDestination = complexSource.MapTo<PersonWithAddress, PersonWithAddressDto>();

            // Assert
            Assert.NotNull(complexDestination);
            Assert.Equal(complexSource.Name, complexDestination.Name);
            Assert.Equal(complexSource.Age, complexDestination.Age);
            Assert.NotNull(complexDestination.Address);
            Assert.Equal(complexSource.Address.Street, complexDestination.Address.Street);
            Assert.Equal(complexSource.Address.City, complexDestination.Address.City);
            Assert.Equal(complexSource.Address.ZipCode, complexDestination.Address.ZipCode);
        }

        [Fact]
        public void MapTo_ComplexObjects_WithDifferentPropsTypesShouldMapNestedProperties()
        {
            // Arrange
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

            // Act
            var complexDestination = complexSource.MapTo<PersonWithAddress, PersonWithAddressDtoAgeDiff>();

            // Assert
            Assert.NotNull(complexDestination);
            Assert.Equal(complexSource.Name, complexDestination.Name);
            Assert.Equal(complexDestination.Age, "35");
            Assert.NotNull(complexDestination.Address);
            Assert.Equal(complexSource.Address.Street, complexDestination.Address.Street);
            Assert.Equal(complexSource.Address.City, complexDestination.Address.City);
            Assert.Equal(complexSource.Address.ZipCode, complexDestination.Address.ZipCode);
        }

        [Fact]
        public void MapTo_ComplexObjects_WithDifferentPropsTypesFromShouldMapNestedProperties()
        {
            // Arrange
            var complexSource = new PersonWithAddressDtoAgeDiff
            {
                Name = "Pedro Costa",
                Age = "35",
                Address = new AddressDto
                {
                    Street = "Rua das Flores, 123",
                    City = "São Paulo",
                    ZipCode = "01234-567"
                }
            };

            // Act
            var complexDestination = complexSource.MapTo<PersonWithAddressDtoAgeDiff, PersonWithAddress>();

            // Assert
            Assert.NotNull(complexDestination);
            Assert.Equal(complexSource.Name, complexDestination.Name);
            Assert.Equal(complexDestination.Age, 35);
            Assert.NotNull(complexDestination.Address);
            Assert.Equal(complexSource.Address.Street, complexDestination.Address.Street);
            Assert.Equal(complexSource.Address.City, complexDestination.Address.City);
            Assert.Equal(complexSource.Address.ZipCode, complexDestination.Address.ZipCode);
        }

        #endregion

        #region Value Object Mapping Tests

        [Fact]
        public void MapTo_ValueObjects_ShouldMapCorrectly()
        {
            // Arrange
            var sourceWithName = new PersonWithCompleteName
            {
                Id = 1,
                Name = new CompleteName("João Silva"),
                Age = 30
            };

            // Act
            var destination = sourceWithName.MapTo<PersonWithCompleteName, PersonWithCompleteNameDto>();

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(sourceWithName.Id, destination.Id);
            Assert.Equal(sourceWithName.Age, destination.Age);
            Assert.NotNull(destination.Name);
            Assert.Equal(sourceWithName.Name.FullName, destination.Name.FullName);
            Assert.Equal(sourceWithName.Name.FirstName, destination.Name.FirstName);
            Assert.Equal(sourceWithName.Name.LastName, destination.Name.LastName);
        }

        [Fact]
        public void MapTo_ValueObjectToString_ShouldUseImplicitConversion()
        {
            // Arrange
            var source = new PersonWithCompleteName
            {
                Id = 1,
                Name = new CompleteName("Maria Santos"),
                Age = 25
            };

            // Act
            var destination = source.MapTo<PersonWithCompleteName, PersonWithStringName>();

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(source.Id, destination.Id);
            Assert.Equal(source.Age, destination.Age);
            Assert.Equal(source.Name.FullName, destination.Name);
        }

        [Fact]
        public void MapTo_StringToValueObject_ShouldUseExplicitConversion()
        {
            // Arrange
            var source = new PersonWithStringName
            {
                Id = 1,
                Name = "Ana Costa",
                Age = 28
            };

            // Act & Assert - This should work if BlazMapper handles explicit conversions
            var destination = source.MapTo<PersonWithStringName, PersonWithCompleteName>();

            Assert.NotNull(destination);
            Assert.Equal(source.Id, destination.Id);
            Assert.Equal(source.Age, destination.Age);
            Assert.NotNull(destination.Name);
            Assert.Equal(source.Name, destination.Name.FullName);
        }

        #endregion

        #region Type Conversion Tests

        [Fact]
        public void MapTo_TypeConversion_ShouldConvertCompatibleTypes()
        {
            // Arrange
            var source = new SourceWithNumbers
            {
                IntValue = 42,
                LongValue = 100L,
                DoubleValue = 3.14,
                StringValue = "123"
            };

            // Act
            var destination = source.MapTo<SourceWithNumbers, DestinationWithNumbers>();

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(source.IntValue, destination.IntValue);
            Assert.Equal(source.LongValue, destination.LongValue);
            Assert.Equal(source.DoubleValue, destination.DoubleValue);
            Assert.Equal(source.StringValue, destination.StringValue);
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void MapTo_IncompatibleTypes_ShouldThrow()
        {
            // Arrange
            var source = new SourcePerson
            {
                Name = "Test",
                Age = 30,
                Email = "test@test.com"
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                source.MapTo<SourcePerson, ImpossibleDestination>());
        }

        #endregion

        #region Anonymous Type Tests

        [Fact]
        public void MapTo_AnonymousToClass_ShouldMapCorrectly()
        {
            // Arrange
            var anonymous = new { Name = "Ana Lima", Age = 28, Department = "TI" };

            // Act
            var destination = anonymous.MapTo<object, Employee>();

            // Assert
            Assert.NotNull(destination);
            Assert.Equal("Ana Lima", destination.Name);
            Assert.Equal(28, destination.Age);
            Assert.Equal("TI", destination.Department);
        }

        #endregion
    }

    #region Test Classes

    // Basic classes
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

    // Records
    public record SourceRecord(string FirstName, string LastName, int Age, string Job);
    public record DestinationRecord(string FirstName, string LastName, int Age, string Job);

    // Records with different parameter order
    public record OrderedRecord(string Name, int Age, string Position);
    public record ReorderedRecord(int Age, string Name, string Position);

    // Complex objects
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

    public class PersonWithAddressDtoAgeDiff
    {
        public string Name { get; set; } = string.Empty;
        public string Age { get; set; }
        public AddressDto Address { get; set; } = new();
    }

    // Value Object classes
    public class PersonWithCompleteName
    {
        public int Id { get; set; }
        public CompleteName Name { get; set; } = new("Unknown Person");
        public int Age { get; set; }
    }

    public class PersonWithCompleteNameDto
    {
        public int Id { get; set; }
        public CompleteName Name { get; set; } = new("Unknown Person");
        public int Age { get; set; }
    }

    public class PersonWithStringName
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    // Type conversion classes
    public class SourceWithNumbers
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public double DoubleValue { get; set; }
        public string StringValue { get; set; } = string.Empty;
    }

    public class DestinationWithNumbers
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public double DoubleValue { get; set; }
        public string StringValue { get; set; } = string.Empty;
    }

    // Employee class
    public class Employee
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Department { get; set; } = string.Empty;
    }

    // Impossible destination for error testing
    public class ImpossibleDestination
    {
        public ImpossibleDestination(ComplexParameter complex)
        {
            Complex = complex;
        }

        public ComplexParameter Complex { get; }
    }

    public class ComplexParameter
    {
        public string Value { get; set; } = string.Empty;
    }

    #endregion
}