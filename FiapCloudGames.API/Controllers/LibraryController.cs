using System.Security.Claims;
using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LibraryController : ControllerBase
{
    private readonly LibraryService _libraryService;
    
    public LibraryController(LibraryService libraryService)
    {
        _libraryService = libraryService;
    }
    
    /// <summary>
    /// Obtém biblioteca do usuário autenticado
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyLibrary()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var library = await _libraryService.GetUserLibraryAsync(userId);
        return Ok(library);
    }
    
    /// <summary>
    /// Adiciona jogo à biblioteca (compra)
    /// </summary>
    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseGame([FromBody] PurchaseGameRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message) = await _libraryService.PurchaseGameAsync(userId, request);
        
        if (!success)
        {
            return BadRequest(new { message });
        }
        
        return Ok(new { message });
    }
    
    /// <summary>
    /// Verifica se usuário possui o jogo
    /// </summary>
    [HttpGet("{gameId}")]
    public async Task<IActionResult> CheckOwnership(Guid gameId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var owns = await _libraryService.UserOwnsGameAsync(userId, gameId);
        return Ok(new { owns });
    }
}
