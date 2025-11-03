namespace FiapCloudGames.Domain.Entities;

public class Promotion
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation property
    public Game Game { get; set; } = null!;
    
    public bool IsValidNow()
    {
        var now = DateTime.UtcNow;
        return IsActive && now >= StartDate && now <= EndDate;
    }
    
    public decimal CalculateDiscountedPrice(decimal originalPrice)
    {
        if (!IsValidNow()) return originalPrice;
        return originalPrice * (1 - DiscountPercentage / 100);
    }
}
