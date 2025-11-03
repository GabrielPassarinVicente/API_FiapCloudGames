using FiapCloudGames.Domain.Validators;
using FluentAssertions;

namespace FiapCloudGames.Tests.Domain;

public class PasswordValidatorTests
{
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordIsNull()
    {
        // Arrange
        string password = null!;
        
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeFalse();
        errorMessage.Should().NotBeEmpty();
    }
    
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordIsTooShort()
    {
        // Arrange
        var password = "Abc1@";
        
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeFalse();
        errorMessage.Should().Contain("mínimo");
    }
    
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoUppercase()
    {
        // Arrange
        var password = "abcdef123@";
        
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeFalse();
        errorMessage.Should().Contain("maiúscula");
    }
    
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoLowercase()
    {
        // Arrange
        var password = "ABCDEF123@";
        
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeFalse();
        errorMessage.Should().Contain("minúscula");
    }
    
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoNumber()
    {
        // Arrange
        var password = "Abcdefgh@";
        
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeFalse();
        errorMessage.Should().Contain("número");
    }
    
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoSpecialCharacter()
    {
        // Arrange
        var password = "Abcdef123";
        
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeFalse();
        errorMessage.Should().Contain("especial");
    }
    
    [Theory]
    [InlineData("Senha123@")]
    [InlineData("MyP@ssw0rd")]
    [InlineData("T3st!ng123")]
    [InlineData("Valid#Pass1")]
    public void IsValid_ShouldReturnTrue_WhenPasswordIsValid(string password)
    {
        // Act
        var result = PasswordValidator.IsValid(password, out string errorMessage);
        
        // Assert
        result.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }
}
