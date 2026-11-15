using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mappeinnlevering_1.Controllers;



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
}