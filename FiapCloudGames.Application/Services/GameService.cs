using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services;

public class GameService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<GameResponse>> GetAllGamesAsync()
    {
        var games = await _unitOfWork.Games.FindAsync(g => g.IsActive);
        var promotions = await _unitOfWork.Promotions.GetAllAsync();
        
        return games.Select(g =>
        {
            var activePromotion = promotions
                .Where(p => p.GameId == g.Id && p.IsValidNow())
                .OrderByDescending(p => p.DiscountPercentage)
                .FirstOrDefault();
            
            decimal? discountedPrice = activePromotion?.CalculateDiscountedPrice(g.Price);
            
            return new GameResponse(
                g.Id,
                g.Title,
                g.Description,
                g.Price,
                discountedPrice,
                g.Genre,
                g.ReleaseDate,
                g.Developer,
                g.Publisher,
                g.IsActive
            );
        });
    }
    
    public async Task<GameResponse?> GetGameByIdAsync(Guid id)
    {
        var game = await _unitOfWork.Games.GetByIdAsync(id);
        if (game == null) return null;
        
        var activePromotion = (await _unitOfWork.Promotions.FindAsync(p => p.GameId == id))
            .Where(p => p.IsValidNow())
            .OrderByDescending(p => p.DiscountPercentage)
            .FirstOrDefault();
        
        decimal? discountedPrice = activePromotion?.CalculateDiscountedPrice(game.Price);
        
        return new GameResponse(
            game.Id,
            game.Title,
            game.Description,
            game.Price,
            discountedPrice,
            game.Genre,
            game.ReleaseDate,
            game.Developer,
            game.Publisher,
            game.IsActive
        );
    }
    
    public async Task<(bool Success, string Message, GameResponse? Data)> CreateGameAsync(CreateGameRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return (false, "Título é obrigatório.", null);
        }
        
        if (request.Price <= 0)
        {
            return (false, "Preço deve ser maior que zero.", null);
        }
        
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Genre = request.Genre,
            ReleaseDate = request.ReleaseDate,
            Developer = request.Developer,
            Publisher = request.Publisher,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveChangesAsync();
        
        var response = new GameResponse(
            game.Id,
            game.Title,
            game.Description,
            game.Price,
            null,
            game.Genre,
            game.ReleaseDate,
            game.Developer,
            game.Publisher,
            game.IsActive
        );
        
        return (true, "Jogo criado com sucesso.", response);
    }
    
    public async Task<(bool Success, string Message)> UpdateGameAsync(Guid id, UpdateGameRequest request)
    {
        var game = await _unitOfWork.Games.GetByIdAsync(id);
        if (game == null)
        {
            return (false, "Jogo não encontrado.");
        }
        
        if (request.Title != null) game.Title = request.Title;
        if (request.Description != null) game.Description = request.Description;
        if (request.Price.HasValue) game.Price = request.Price.Value;
        if (request.Genre != null) game.Genre = request.Genre;
        if (request.ReleaseDate.HasValue) game.ReleaseDate = request.ReleaseDate.Value;
        if (request.Developer != null) game.Developer = request.Developer;
        if (request.Publisher != null) game.Publisher = request.Publisher;
        if (request.IsActive.HasValue) game.IsActive = request.IsActive.Value;
        
        game.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Games.UpdateAsync(game);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Jogo atualizado com sucesso.");
    }
    
    public async Task<(bool Success, string Message)> DeleteGameAsync(Guid id)
    {
        var game = await _unitOfWork.Games.GetByIdAsync(id);
        if (game == null)
        {
            return (false, "Jogo não encontrado.");
        }
        
        game.IsActive = false;
        game.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Games.UpdateAsync(game);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Jogo desativado com sucesso.");
    }
}
