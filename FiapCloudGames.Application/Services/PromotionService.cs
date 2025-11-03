using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services;

public class PromotionService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public PromotionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<PromotionResponse>> GetActivePromotionsAsync()
    {
        var promotions = await _unitOfWork.Promotions.GetAllAsync();
        var activePromotions = promotions.Where(p => p.IsValidNow());
        
        var gameIds = activePromotions.Select(p => p.GameId).Distinct().ToList();
        var games = await _unitOfWork.Games.GetAllAsync();
        var gamesDict = games.Where(g => gameIds.Contains(g.Id)).ToDictionary(g => g.Id);
        
        return activePromotions.Select(p => new PromotionResponse(
            p.Id,
            p.GameId,
            gamesDict.ContainsKey(p.GameId) ? gamesDict[p.GameId].Title : "Desconhecido",
            p.DiscountPercentage,
            p.StartDate,
            p.EndDate,
            p.IsActive
        ));
    }
    
    public async Task<PromotionResponse?> GetPromotionByIdAsync(Guid id)
    {
        var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
        if (promotion == null) return null;
        
        var game = await _unitOfWork.Games.GetByIdAsync(promotion.GameId);
        
        return new PromotionResponse(
            promotion.Id,
            promotion.GameId,
            game?.Title ?? "Desconhecido",
            promotion.DiscountPercentage,
            promotion.StartDate,
            promotion.EndDate,
            promotion.IsActive
        );
    }
    
    public async Task<(bool Success, string Message, PromotionResponse? Data)> CreatePromotionAsync(CreatePromotionRequest request)
    {
        // Validações
        var game = await _unitOfWork.Games.GetByIdAsync(request.GameId);
        if (game == null)
        {
            return (false, "Jogo não encontrado.", null);
        }
        
        if (request.DiscountPercentage <= 0 || request.DiscountPercentage > 100)
        {
            return (false, "Percentual de desconto deve estar entre 0 e 100.", null);
        }
        
        if (request.StartDate >= request.EndDate)
        {
            return (false, "Data de início deve ser anterior à data de término.", null);
        }
        
        var promotion = new Promotion
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            DiscountPercentage = request.DiscountPercentage,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true
        };
        
        await _unitOfWork.Promotions.AddAsync(promotion);
        await _unitOfWork.SaveChangesAsync();
        
        var response = new PromotionResponse(
            promotion.Id,
            promotion.GameId,
            game.Title,
            promotion.DiscountPercentage,
            promotion.StartDate,
            promotion.EndDate,
            promotion.IsActive
        );
        
        return (true, "Promoção criada com sucesso.", response);
    }
    
    public async Task<(bool Success, string Message)> UpdatePromotionAsync(Guid id, UpdatePromotionRequest request)
    {
        var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
        if (promotion == null)
        {
            return (false, "Promoção não encontrada.");
        }
        
        if (request.DiscountPercentage.HasValue)
        {
            if (request.DiscountPercentage.Value <= 0 || request.DiscountPercentage.Value > 100)
            {
                return (false, "Percentual de desconto deve estar entre 0 e 100.");
            }
            promotion.DiscountPercentage = request.DiscountPercentage.Value;
        }
        
        if (request.StartDate.HasValue) promotion.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) promotion.EndDate = request.EndDate.Value;
        if (request.IsActive.HasValue) promotion.IsActive = request.IsActive.Value;
        
        if (promotion.StartDate >= promotion.EndDate)
        {
            return (false, "Data de início deve ser anterior à data de término.");
        }
        
        await _unitOfWork.Promotions.UpdateAsync(promotion);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Promoção atualizada com sucesso.");
    }
    
    public async Task<(bool Success, string Message)> DeletePromotionAsync(Guid id)
    {
        var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
        if (promotion == null)
        {
            return (false, "Promoção não encontrada.");
        }
        
        await _unitOfWork.Promotions.DeleteAsync(promotion);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Promoção deletada com sucesso.");
    }
}
