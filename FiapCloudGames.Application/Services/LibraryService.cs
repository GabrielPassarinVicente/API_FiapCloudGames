using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services;

public class LibraryService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public LibraryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<UserGameResponse>> GetUserLibraryAsync(Guid userId)
    {
        var userGames = await _unitOfWork.UserGames.FindAsync(ug => ug.UserId == userId);
        var gameIds = userGames.Select(ug => ug.GameId).ToList();
        var games = await _unitOfWork.Games.GetAllAsync();
        var gamesDict = games.Where(g => gameIds.Contains(g.Id)).ToDictionary(g => g.Id);
        
        return userGames.Select(ug => new UserGameResponse(
            ug.Id,
            ug.GameId,
            gamesDict.ContainsKey(ug.GameId) ? gamesDict[ug.GameId].Title : "Desconhecido",
            ug.PurchaseDate,
            ug.PurchasePrice
        ));
    }
    
    public async Task<(bool Success, string Message)> PurchaseGameAsync(Guid userId, PurchaseGameRequest request)
    {
        // Verificar se o jogo existe
        var game = await _unitOfWork.Games.GetByIdAsync(request.GameId);
        if (game == null)
        {
            return (false, "Jogo não encontrado.");
        }
        
        if (!game.IsActive)
        {
            return (false, "Jogo não está disponível para compra.");
        }
        
        // Verificar se o usuário já possui o jogo
        var existingPurchase = await _unitOfWork.UserGames.FirstOrDefaultAsync(
            ug => ug.UserId == userId && ug.GameId == request.GameId
        );
        
        if (existingPurchase != null)
        {
            return (false, "Você já possui este jogo.");
        }
        
        // Verificar se há promoção ativa
        var activePromotion = (await _unitOfWork.Promotions.FindAsync(p => p.GameId == request.GameId))
            .Where(p => p.IsValidNow())
            .OrderByDescending(p => p.DiscountPercentage)
            .FirstOrDefault();
        
        var purchasePrice = activePromotion?.CalculateDiscountedPrice(game.Price) ?? game.Price;
        
        // Criar registro de compra
        var userGame = new UserGame
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            GameId = request.GameId,
            PurchaseDate = DateTime.UtcNow,
            PurchasePrice = purchasePrice
        };
        
        await _unitOfWork.UserGames.AddAsync(userGame);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Jogo adicionado à biblioteca com sucesso.");
    }
    
    public async Task<bool> UserOwnsGameAsync(Guid userId, Guid gameId)
    {
        return await _unitOfWork.UserGames.ExistsAsync(ug => ug.UserId == userId && ug.GameId == gameId);
    }
}
