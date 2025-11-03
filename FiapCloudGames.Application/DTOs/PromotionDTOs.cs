namespace FiapCloudGames.Application.DTOs;

public record PromotionResponse(
    Guid Id,
    Guid GameId,
    string GameTitle,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
);

public record CreatePromotionRequest(
    Guid GameId,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);

public record UpdatePromotionRequest(
    decimal? DiscountPercentage,
    DateTime? StartDate,
    DateTime? EndDate,
    bool? IsActive
);
