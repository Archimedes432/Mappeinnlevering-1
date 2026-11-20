using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Services;
using Mappeinnlevering_1.Tests.Mocks;

namespace Mappeinnlevering_1.Tests.BusinessRules;

public class ListingServiceTests
{
    private readonly MockListingRepository _listings = new MockListingRepository();
    private readonly ListingService _service;

    public ListingServiceTests()
    {
        _service = new ListingService(_listings);
    }

    // 6. Prisendring lager en PriceChange med riktig gammel og ny pris
    [Fact]
    public async Task PriceChange_IsLogged()
    {
        var listing = _listings.Add("selger", 1000);
        var dto = new UpdateListingDto { Title = "Test", Price = 800, Condition = Condition.Good };

        await _service.UpdateAsync(listing.Id, dto, "selger", false);

        var change = Assert.Single(_listings.PriceChanges);
        Assert.Equal(1000, change.OldPrice);
        Assert.Equal(800, change.NewPrice);
    }

    // 7. Endring av solgt annonse avvises
    [Fact]
    public async Task UpdateSoldListing_IsRejected()
    {
        var listing = _listings.Add("selger", 1000, ListingStatus.Sold);
        var dto = new UpdateListingDto { Title = "Test", Price = 800, Condition = Condition.Good };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(listing.Id, dto, "selger", false));
    }
}
