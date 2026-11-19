

using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;

namespace Mappeinnlevering_1.Services.Interfaces;

public interface IAnalyticsService
{
    Task<PriceSuggestionDto?> GetPriceSuggestionAsync(int categoryId, Condition condition);
    Task<PriceEvaluationDto?> EvaluateListingPriceAsync(int listingId);

    Task<CategoryStatsDto?> GetCategoryStatsAsync(int categoryId);

    Task<List<CategoryStatsDto>> GetOverviewAsync();

    Task<PriceHistoryDto> GetPriceHistoryAsync(int listingId);
}