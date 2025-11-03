namespace FiapCloudGames.Application.DTOs;

public record UserGameResponse(
    Guid Id,
    Guid GameId,
    string GameTitle,
    DateTime PurchaseDate,
    decimal PurchasePrice
);

public record PurchaseGameRequest(
    Guid GameId
);
