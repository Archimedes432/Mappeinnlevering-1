using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Mappeinnlevering_1.Services.Interfaces;

namespace Mappeinnlevering_1.Services;

public class AnalyticsService : IAnalyticsService
{
  private const int MinSalesForSuggestion = 3;
  
  private readonly IAnalyticsRepository _repo;

  public AnalyticsService(IAnalyticsRepository repo)
  {
    _repo = repo;
  }

  public async Task<PriceSuggestionDto?> GetPriceSuggestionAsync(int categoryId, Condition condition)
  {
    Category? category = await _repo.GetCategoryAsync(categoryId);
    if (category == null)
    {
      return null;
    }

    List<Listing> sold = await _repo.GetSoldListingsAsync(categoryId);

    PriceSuggestionDto result = new PriceSuggestionDto();
    result.CategoryId = categoryId;
    result.Condition = condition;
    result.NumberOfSales = sold.Count;

    if (sold.Count < MinSalesForSuggestion)
    {
      result.HasEnoughData = false;
      result.Message = "Ikke nok data: krever minst 3 salg i kategorien.";
      return result;
    }

    List<double> normalizedPrices = NormalizedSoldPrices(sold);
    double median = Statistics.Median(normalizedPrices);
    double price = median * ConditionFactors.Get(condition);

    result.HasEnoughData = true;
    result.SuggestedPrice = RoundToNearest10(price);
    return result;
  }
  
  // Hjelpemetoder
  private static List<double> NormalizedSoldPrices(List<Listing> sold)
  {
    List<double> result = new List<double>();
    foreach (Listing l in sold)
    {
      int soldPrice = l.SoldPrice.GetValueOrDefault();
      result.Add(ConditionFactors.Normalize(soldPrice, l.Condition));
    }
    return result;
  }


  private static int RoundToNearest10(double value)
  {
    double rounded = Math.Round(value / 10, MidpointRounding.AwayFromZero) * 10;
    return (int)rounded;
  }


}