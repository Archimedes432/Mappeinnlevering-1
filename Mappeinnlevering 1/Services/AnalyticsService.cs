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

  public async Task<CategoryStatsDto?> GetCategoryStatsAsync(int categoryId)
  {
    Category? category = await _repo.GetCategoryAsync(categoryId);
    if (category == null)
    {
      return null;
    }

    List<Listing> listings = await _repo.GetListingsInCategoryAsync(categoryId);

    List<Listing> sold = listings
      .Where(l => l.Status == ListingStatus.Sold && l.SoldPrice != null && l.SoldDate != null)
      .ToList();

    int active = listings.Count(l => l.Status == ListingStatus.Active);
    int removed = listings.Count(l => l.Status == ListingStatus.Removed);

    CategoryStatsDto stats = new CategoryStatsDto();
    stats.CategoryId = category.Id;
    stats.CategoryName = category.Name;
    stats.ActiveCount = active;
    stats.SoldCount = sold.Count;
    stats.RemoveCount = removed;

    int total = sold.Count + active + removed;
    if (total > 0)
    {
      stats.SalesRate = Math.Round((double)sold.Count / total, 4);
    }

    if (sold.Count == 0)
    {
      return stats;
    }

    List<double> soldPrices = new List<double>();
    List<double> daysToSell = new List<double>();
    List<double> discounts = new List<double>();

    foreach (Listing l in sold)
    {
      int soldPrice = l.SoldPrice.GetValueOrDefault();
      DateTime soldDate = l.SoldDate.GetValueOrDefault();
      int original = OriginalPrice(l);

      soldPrices.Add(soldPrice);
      daysToSell.Add((soldDate - l.CreatedDate).TotalDays);
      discounts.Add((original - soldPrice) * 100.00 / original);
    }

    stats.AverageSoldPrice = Math.Round(Statistics.Mean(soldPrices), 2);
    stats.MedianSoldPrice = Math.Round(Statistics.Median(soldPrices), 2);
    stats.AverageDaysToSell = Math.Round(daysToSell.Average(), 2);
    stats.AverageDiscountPercent = Math.Round(discounts.Average(), 2);

    if (soldPrices.Count >= 2)
    {
      stats.StandardDeviationSoldPrice = Math.Round(Statistics.StandardDeviation(soldPrices), 2);
    }
    return stats;
  }

  public async Task<List<CategoryStatsDto>> GetOverviewAsync()
  {
    List<CategoryStatsDto> result = new List<CategoryStatsDto>();
    List<Category> categories = await _repo.GetCategoriesAsync();

    foreach (Category category in categories)
    {
      CategoryStatsDto? stats = await GetCategoryStatsAsync(category.Id);
      if (stats != null)
      {
        result.Add(stats);
      }
    }
    return result;
  }

  public async Task<PriceHistoryDto> GetPriceHistoryAsync(int listingId)
  {
    Listing? listing = await _repo.GetListingAsync(listingId);
    if (listing == null)
    {
      return null;
    }

    PriceHistoryDto result = new PriceHistoryDto();
    result.ListingId = listing.Id;
    result.CurrentPrice = listing.Price;

    foreach (PriceChange pc in listing.PriceChanges.OrderBy(pc => pc.ChangedDate))
    {
      PriceChangeDto change = new PriceChangeDto();
      change.OldPrice = pc.OldPrice;
      change.NewPrice = pc.NewPrice;
      change.ChangedDate = pc.ChangedDate;
      result.Changes.Add(change);
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

  private static int OriginalPrice(Listing listing)
  {
    if (listing.PriceChanges.Count == 0)
    {
      return listing.Price; 
    }

    PriceChange first = listing.PriceChanges.OrderBy(pc => pc.ChangedDate).First();
    return first.OldPrice;
    
  }

  
  
}