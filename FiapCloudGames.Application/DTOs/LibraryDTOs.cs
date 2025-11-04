namespace FiapCloudGames.Application.DTOs;

public record UserGameResponse(
    Guid Id,
    Guid GameId,
    string GameTitle,
    DateTime PurchaseDate,
    decimal PurchasePrice
);

// record class com propriedade settable
public record class PurchaseGameRequest
{
    public Guid GameId { get; init; }
}
