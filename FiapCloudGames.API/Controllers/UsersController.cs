using System.Security.Claims;
using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    
    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Obtém dados do usuário autenticado
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }
        
        return Ok(user);
    }
    
    /// <summary>
    /// Atualiza dados do usuário autenticado
    /// </summary>
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message) = await _userService.UpdateUserAsync(userId, request);
        
        if (!success)
        {
            return BadRequest(new { message });
        }
        
        return Ok(new { message });
    }
    
    /// <summary>
    /// Obtém usuário por ID (apenas Admin)
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }
        
        return Ok(user);
    }
    
    /// <summary>
    /// Lista todos os usuários (apenas Admin)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    /// <summary>
    /// Deleta usuário (apenas Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, message) = await _userService.DeleteUserAsync(id);
        
        if (!success)
        {
            return NotFound(new { message });
        }
        
        return Ok(new { message });
    }
}
