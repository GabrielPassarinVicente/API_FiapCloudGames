namespace FiapCloudGames.Domain.Entities;

public class UserGame
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
