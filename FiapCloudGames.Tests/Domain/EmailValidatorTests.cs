using FiapCloudGames.Domain.Validators;
using FluentAssertions;

namespace FiapCloudGames.Tests.Domain;

public class EmailValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ShouldReturnFalse_WhenEmailIsNullOrWhitespace(string email)
    {
        // Act
        var result = EmailValidator.IsValid(email);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData("invalidemail")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user @example.com")]
    [InlineData("user@.com")]
    public void IsValid_ShouldReturnFalse_WhenEmailFormatIsInvalid(string email)
    {
        // Act
        var result = EmailValidator.IsValid(email);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.user@domain.com")]
    [InlineData("admin@fiap.com.br")]
    [InlineData("contact+tag@company.co")]
    public void IsValid_ShouldReturnTrue_WhenEmailFormatIsValid(string email)
    {
        // Act
        var result = EmailValidator.IsValid(email);
        
        // Assert
        result.Should().BeTrue();
    }
}
