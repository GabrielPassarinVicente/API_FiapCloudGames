using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra um novo usuário
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (success, message, data) = await _authService.RegisterAsync(request);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message, data });
    }

    /// <summary>
    /// Realiza login e retorna token JWT
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, message, data) = await _authService.LoginAsync(request);

        if (!success)
        {
            return Unauthorized(new { message });
        }

        return Ok(new { message, data });
    }
}
