using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mappeinnlevering_1.Services;

public class AnalyticsService : IAnalyticsService
{
  private const int MinSalesForSuggestion = 3;
  private const int MinSalesForEvaluation = 5;
  
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

  public async Task<PriceEvaluationDto?> EvaluateListingPriceAsync(int listingId)
  {
    Listing? listing = await _repo.GetListingAsync(listingId);
    if (listing == null)
    {
      return null;
    }

    List<Listing> sold = await _repo.GetSoldListingsAsync(listing.CategoryId);
    double normalized = ConditionFactors.Normalize(listing.Price, listing.Condition);

    PriceEvaluationDto result = new PriceEvaluationDto();
    result.ListingId = listing.Id;
    result.NormalizedPrice = Math.Round(normalized, 2);
    result.NumberOfSales = sold.Count;

    if (sold.Count < MinSalesForEvaluation)
    {
      result.Rating = PriceRating.Unknown;
      return result;
    }

    List<double> prices = NormalizedSoldPrices(sold);
    double q1 = Statistics.Quartile(prices, 0.25);
    double q3 = Statistics.Quartile(prices, 0.75);
    double iqr = q3 - q1;

    double lower = q1 - 1.5 * iqr;
    double upper = q3 + 1.5 * iqr;

    result.LowerLimit = Math.Round(lower, 2);
    result.UpperLimit = Math.Round(upper, 2);

    if (normalized < lower)
    {
      result.Rating = PriceRating.Low;
    }
    else if (normalized > upper)
    {
      result.Rating = PriceRating.High;
    }
    else
    {
      result.Rating = PriceRating.Normal;
    }

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