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

// record class com propriedades settable (melhor para model binding)
                public record class CreatePromotionRequest
                {
                    public Guid GameId { get; init; }
                    public decimal DiscountPercentage { get; init; }
                    public DateTime StartDate { get; init; }
                    public DateTime EndDate { get; init; }
                }

                public record class UpdatePromotionRequest
                {
                    public decimal? DiscountPercentage { get; init; }
                    public DateTime? StartDate { get; init; }
                    public DateTime? EndDate { get; init; }
                    public bool? IsActive { get; init; }
                }
