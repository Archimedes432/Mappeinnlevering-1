using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Services;
using Mappeinnlevering_1.Tests.Mocks;

namespace Mappeinnlevering_1.Tests.Analytics;

public class AnalyticsServiceTests
{
    private readonly MockAnalyticsRepository _repo;
    private readonly AnalyticsService _service;

    public AnalyticsServiceTests()
    {
        _repo = new MockAnalyticsRepository();
        _service = new AnalyticsService(_repo);
    }

    // 3. Prisforslaget blir riktig, inkludert normalisering og avrunding
    [Fact]
    public async Task PriceSuggestion_NormalizesAndRounds()
    {
        // Normaliserte priser: 1000/1.0 = 1000, 1234/1.0 = 1234, 1050/0.70 = 1500
        // Median = 1234. Ønsket tilstand LikeNew: 1234 * 0.85 = 1048.9 -> 1050
        _repo.AddSold(1000, Condition.New);
        _repo.AddSold(1234, Condition.New);
        _repo.AddSold(1050, Condition.Good);

        PriceSuggestionDto? result = await _service.GetPriceSuggestionAsync(1, Condition.LikeNew);

        Assert.NotNull(result);
        Assert.True(result.HasEnoughData);
        Assert.Equal(1050, result.SuggestedPrice);
    }

    // 4. Prisforslag med færre enn 3 salg gir «ikke nok data»
    [Fact]
    public async Task PriceSuggestion_FewerThan3Sales_NotEnoughData()
    {
        _repo.AddSold(1000, Condition.New);
        _repo.AddSold(1200, Condition.New);

        PriceSuggestionDto? result = await _service.GetPriceSuggestionAsync(1, Condition.Good);

        Assert.NotNull(result);
        Assert.False(result.HasEnoughData);
        Assert.Null(result.SuggestedPrice);
    }

    // 5. salg (alle New): 1000, 1100, 1200, 1300, 1400
    // Q1 = 1100, Q3 = 1300, IQR = 200 -> grenser: 1100 - 300 = 800 og 1300 + 300 = 1600
    private void AddFiveSales()
    {
        _repo.AddSold(1000, Condition.New);
        _repo.AddSold(1100, Condition.New);
        _repo.AddSold(1200, Condition.New);
        _repo.AddSold(1300, Condition.New);
        _repo.AddSold(1400, Condition.New);
    }

    // 5. Prisvurdering: lav pris gir Lav
    [Fact]
    public async Task Evaluation_LowPrice_IsLow()
    {
        AddFiveSales();
        Listing listing = _repo.AddActive(500, Condition.New);      // 500 < 800

        PriceEvaluationDto? result = await _service.EvaluateListingPriceAsync(listing.Id);

        Assert.NotNull(result);
        Assert.Equal(PriceRating.Low, result.Rating);
    }

    // 5. Prisvurdering: normal pris gir Normal
    [Fact]
    public async Task Evaluation_NormalPrice_IsNormal()
    {
        AddFiveSales();
        Listing listing = _repo.AddActive(700, Condition.Good);     // 700 / 0.70 = 1000

        PriceEvaluationDto? result = await _service.EvaluateListingPriceAsync(listing.Id);

        Assert.NotNull(result);
        Assert.Equal(PriceRating.Normal, result.Rating);
    }

    // 5. Prisvurdering: høy pris gir Høy
    [Fact]
    public async Task Evaluation_HighPrice_IsHigh()
    {
        AddFiveSales();
        Listing listing = _repo.AddActive(5000, Condition.New);     // 5000 > 1600

        PriceEvaluationDto? result = await _service.EvaluateListingPriceAsync(listing.Id);

        Assert.NotNull(result);
        Assert.Equal(PriceRating.High, result.Rating);
    }

    // 5. Prisvurdering: færre enn 5 salg gir Ukjent
    [Fact]
    public async Task Evaluation_FewerThan5Sales_IsUnknown()
    {
        // Bare 4 salg
        _repo.AddSold(1000, Condition.New);
        _repo.AddSold(1100, Condition.New);
        _repo.AddSold(1200, Condition.New);
        _repo.AddSold(1300, Condition.New);
        Listing listing = _repo.AddActive(5000, Condition.New);

        PriceEvaluationDto? result = await _service.EvaluateListingPriceAsync(listing.Id);

        Assert.NotNull(result);
        Assert.Equal(PriceRating.Unknown, result.Rating);
    }

    // 6. Gjennomsnittlig rabatt bruker opprinnelig pris når prisen er endret
    [Fact]
    public async Task Stats_AverageDiscount_UsesOriginalPriceWhenChanged()
    {
        // A: 1000 -> 950 -> 900, solgt for 800. Opprinnelig = 1000 -> rabatt 20 %
        Listing a = _repo.AddSold(800, Condition.Good, 900);
        a.PriceChanges.Add(new PriceChange { OldPrice = 950, NewPrice = 900, ChangedDate = new DateTime(2026, 1, 5) });
        a.PriceChanges.Add(new PriceChange { OldPrice = 1000, NewPrice = 950, ChangedDate = new DateTime(2026, 1, 3) });

        // B: aldri endret, pris 500, solgt for 450 -> rabatt 10 %
        _repo.AddSold(450, Condition.Good, 500);

        CategoryStatsDto? stats = await _service.GetCategoryStatsAsync(1);

        // (20 + 10) / 2 = 15
        Assert.NotNull(stats);
        Assert.Equal(15, stats.AverageDiscountPercent);
    }
}

