using Domain.Primitives;

namespace Tests.UnitTests;

public class ValueObjectTests
{
    private class TestValueObject : ValueObject<int, TestValueObject>
    {
        protected override void Validate()
        {
            if (Value < 0)
            {
                throw new Exception("Value cannot be negative.");
            }
            base.Validate();

        }
    
    }

    [Fact]
    public void From_ShouldCreateValueObject()
    {
        var value = 5;
        var valueObject = TestValueObject.From(value);

        Assert.Equal(value, valueObject.Value);
    }

    [Fact]
    public void EqualityOperator_ShouldCompareValueObjectsCorrectly()
    {
        var valueObject1 = TestValueObject.From(5);
        var valueObject2 = TestValueObject.From(5);
        var valueObject3 = TestValueObject.From(6);

        Assert.True(valueObject1 == valueObject2);
        Assert.False(valueObject1 == valueObject3);
    }
    
    [Fact]
    public void Equals_WhenCalledWithEqualObjects_ReturnsTrue()
    {
        // Arrange
        var value1 = TestValueObject.From(1);
        var value2 = TestValueObject.From(1);

        // Act
        var result = value1.Equals(value2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenCalledWithDifferentObjects_ReturnsFalse()
    {
        // Arrange
        var value1 = TestValueObject.From(1);
        var value2 = TestValueObject.From(2);

        // Act
        var result = value1.Equals(value2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenCalledWithNull_ReturnsFalse()
    {
        // Arrange
        var value1 = TestValueObject.From(1);

        // Act
        var result = value1.Equals(null);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Equals_WhenCalledWithSameReference_ReturnsTrue()
    {
        // Arrange
        var valueObject1 = TestValueObject.From(1);

        // Act
        var result = valueObject1.Equals(valueObject1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenCalledWithDifferentReference_ReturnsFalse()
    {
        // Arrange
        var valueObject1 = TestValueObject.From(1);
        var valueObject2 = TestValueObject.From(2);

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void GetHashCode_ReturnsSameHashCodeForEqualObjects()
    {
        // Arrange
        var value1 = TestValueObject.From(1);
        var value2 = TestValueObject.From(1);

        // Act
        var hash1 = value1.GetHashCode();
        var hash2 = value2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_ReturnsDifferentHashCodesForDifferentObjects()
    {
        // Arrange
        var value1 = TestValueObject.From(1);
        var value2 = TestValueObject.From(2);

        // Act
        var hash1 = value1.GetHashCode();
        var hash2 = value2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }
    
    [Fact]
    public void TryFrom_ValidInput_ReturnsTrueAndSetsValue()
    {
        // Arrange
        var value = 1;

        // Act
        bool result = TestValueObject.TryFrom(value, out var valueObject);

        // Assert
        Assert.True(result);
        Assert.Equal(value, valueObject.Value);
    }
   
    [Fact]
    public void ToString_ReturnsCorrectStringRepresentation()
    {
        // Arrange
        var value = 1;
        var valueObject = TestValueObject.From(value);

        // Act
        var result = valueObject.ToString();

        // Assert
        Assert.Equal(value.ToString(), result);
    }
    
    [Fact]
    public void From_WhenValueIsNegative_ThrowsException()
    {
        // Arrange
        var value = -1;

        // Act & Assert
        Assert.Throws<Exception>(() => TestValueObject.From(value));
    }

    [Fact]
    public void From_WhenValueIsGreaterThanNine_DoesNotThrowException()
    {
        // Arrange
        var value = 10;

        // Act & Assert
        var exception = Record.Exception(() => TestValueObject.From(value));
        Assert.Null(exception);
    }
}
