using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mappeinnlevering_1.Controllers;


[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analytics;

    public AnalyticsController (IAnalyticsService analytics)
    {
        _analytics = analytics;
    }

    [Authorize]
    [HttpGet("price-suggestion")]
    public async Task<ActionResult<PriceSuggestionDto>> GetPriceSuggestion(
        [FromQuery, BindRequired] int categoryId,
        [FromQuery, BindRequired] Condition condition)
    {
        PriceSuggestionDto? result = await _analytics.GetPriceSuggestionAsync(categoryId, condition);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }


    [HttpGet("categories/{id:int}/stats")]
    public async Task<ActionResult<CategoryStatsDto>> GetCategoryStats(int id)
    {
        CategoryStatsDto? result = await _analytics.GetCategoryStatsAsync(id);
        
        if (result == null) 
        { 
            return NotFound();  
        } 
        return Ok(result);
        }
    
    [HttpGet("overview")]
    public async Task<ActionResult<List<CategoryStatsDto>>> GetOverview()
    {
        List<CategoryStatsDto> result = await _analytics.GetOverviewAsync();
        return Ok(result);
    }
    
    [HttpGet("listings/{id:int}/price-history")]
    public async Task<ActionResult<PriceHistoryDto>> GetPriceHistory(int id)
    {
        PriceHistoryDto? result = await _analytics.GetPriceHistoryAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }


}