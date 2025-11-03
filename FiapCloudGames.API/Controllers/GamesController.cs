using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameService _gameService;
    
    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }
    
    /// <summary>
    /// Lista todos os jogos ativos
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }
    
    /// <summary>
    /// Obtém jogo por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        
        if (game == null)
        {
            return NotFound(new { message = "Jogo não encontrado." });
        }
        
        return Ok(game);
    }
    
    /// <summary>
    /// Cadastra novo jogo (apenas Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateGameRequest request)
    {
        var (success, message, data) = await _gameService.CreateGameAsync(request);
        
        if (!success)
        {
            return BadRequest(new { message });
        }
        
        return CreatedAtAction(nameof(GetById), new { id = data!.Id }, new { message, data });
    }
    
    /// <summary>
    /// Atualiza jogo (apenas Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameRequest request)
    {
        var (success, message) = await _gameService.UpdateGameAsync(id, request);
        
        if (!success)
        {
            return NotFound(new { message });
        }
        
        return Ok(new { message });
    }
    
    /// <summary>
    /// Desativa jogo (apenas Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, message) = await _gameService.DeleteGameAsync(id);
        
        if (!success)
        {
            return NotFound(new { message });
        }
        
        return Ok(new { message });
    }
}
