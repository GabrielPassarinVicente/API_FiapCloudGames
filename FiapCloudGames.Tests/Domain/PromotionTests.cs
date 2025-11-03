using FiapCloudGames.Domain.Entities;
using FluentAssertions;

namespace FiapCloudGames.Tests.Domain;

public class PromotionTests
{
    [Fact]
    public void IsValidNow_ShouldReturnTrue_WhenPromotionIsActiveAndWithinDateRange()
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1)
        };
        
        // Act
        var result = promotion.IsValidNow();
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void IsValidNow_ShouldReturnFalse_WhenPromotionIsNotActive()
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = false,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1)
        };
        
        // Act
        var result = promotion.IsValidNow();
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void IsValidNow_ShouldReturnFalse_WhenPromotionHasNotStarted()
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2)
        };
        
        // Act
        var result = promotion.IsValidNow();
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void IsValidNow_ShouldReturnFalse_WhenPromotionHasEnded()
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(-2),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        
        // Act
        var result = promotion.IsValidNow();
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void CalculateDiscountedPrice_ShouldApplyDiscount_WhenPromotionIsValid()
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1),
            DiscountPercentage = 20
        };
        var originalPrice = 100m;
        
        // Act
        var discountedPrice = promotion.CalculateDiscountedPrice(originalPrice);
        
        // Assert
        discountedPrice.Should().Be(80m);
    }
    
    [Fact]
    public void CalculateDiscountedPrice_ShouldReturnOriginalPrice_WhenPromotionIsNotValid()
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = false,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1),
            DiscountPercentage = 20
        };
        var originalPrice = 100m;
        
        // Act
        var discountedPrice = promotion.CalculateDiscountedPrice(originalPrice);
        
        // Assert
        discountedPrice.Should().Be(originalPrice);
    }
    
    [Theory]
    [InlineData(100, 10, 90)]
    [InlineData(100, 25, 75)]
    [InlineData(100, 50, 50)]
    [InlineData(100, 75, 25)]
    [InlineData(50.50, 20, 40.40)]
    public void CalculateDiscountedPrice_ShouldCalculateCorrectly_WithDifferentValues(
        decimal originalPrice, decimal discountPercentage, decimal expectedPrice)
    {
        // Arrange
        var promotion = new Promotion
        {
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1),
            DiscountPercentage = discountPercentage
        };
        
        // Act
        var discountedPrice = promotion.CalculateDiscountedPrice(originalPrice);
        
        // Assert
        discountedPrice.Should().BeApproximately(expectedPrice, 0.01m);
    }
}
