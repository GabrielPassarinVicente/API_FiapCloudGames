using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly PromotionService _promotionService;
    
    public PromotionsController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    
    /// <summary>
    /// Lista promoções ativas
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActivePromotions()
    {
        var promotions = await _promotionService.GetActivePromotionsAsync();
        return Ok(promotions);
    }
    
    /// <summary>
    /// Obtém promoção por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var promotion = await _promotionService.GetPromotionByIdAsync(id);
        
        if (promotion == null)
        {
            return NotFound(new { message = "Promoção não encontrada." });
        }
        
        return Ok(promotion);
    }
    
    /// <summary>
    /// Cria nova promoção (apenas Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
    {
        var (success, message, data) = await _promotionService.CreatePromotionAsync(request);
        
        if (!success)
        {
            return BadRequest(new { message });
        }
        
        return CreatedAtAction(nameof(GetById), new { id = data!.Id }, new { message, data });
    }
    
    /// <summary>
    /// Atualiza promoção (apenas Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePromotionRequest request)
    {
        var (success, message) = await _promotionService.UpdatePromotionAsync(id, request);
        
        if (!success)
        {
            return BadRequest(new { message });
        }
        
        return Ok(new { message });
    }
    
    /// <summary>
    /// Deleta promoção (apenas Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, message) = await _promotionService.DeletePromotionAsync(id);
        
        if (!success)
        {
            return NotFound(new { message });
        }
        
        return Ok(new { message });
    }
}
