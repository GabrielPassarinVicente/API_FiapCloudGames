namespace FiapCloudGames.Application.DTOs;

public record GameResponse(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    decimal? DiscountedPrice,
    string Genre,
    DateTime ReleaseDate,
    string Developer,
    string Publisher,
    bool IsActive
);

public record CreateGameRequest(
    string Title,
    string Description,
    decimal Price,
    string Genre,
    DateTime ReleaseDate,
    string Developer,
    string Publisher
);

public record UpdateGameRequest(
    string? Title,
    string? Description,
    decimal? Price,
    string? Genre,
    DateTime? ReleaseDate,
    string? Developer,
    string? Publisher,
    bool? IsActive
);
