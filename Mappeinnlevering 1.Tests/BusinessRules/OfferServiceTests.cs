using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Services;
using Mappeinnlevering_1.Tests.Mocks;

namespace Mappeinnlevering_1.Tests.BusinessRules;

public class OfferServiceTests
{
    private readonly MockListingRepository _listings = new MockListingRepository();
    private readonly MockOfferRepository _offers = new MockOfferRepository();
    private readonly OfferService _service;

    public OfferServiceTests()
    {
        _service = new OfferService(_offers, _listings);
    }

    // 1. Bud på egen annonse avvises
    [Fact]
    public async Task Offer_OnOwnListing_IsRejected()
    {
        var listing = _listings.Add("selger", 1000);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(listing.Id, "selger", new CreateOfferDto { Amount = 800 }));
    }

    // 2. Bud på solgt annonse avvises
    [Fact]
    public async Task Offer_OnSoldListing_IsRejected()
    {
        var listing = _listings.Add("selger", 1000, ListingStatus.Sold);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(listing.Id, "kjoper", new CreateOfferDto { Amount = 800 }));
    }

    // 3. Et andre Pending-bud fra samme kjøper avvises
    [Fact]
    public async Task SecondPendingOffer_IsRejected()
    {
        var listing = _listings.Add("selger", 1000);
        await _service.CreateAsync(listing.Id, "kjoper", new CreateOfferDto { Amount = 700 });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(listing.Id, "kjoper", new CreateOfferDto { Amount = 800 }));
    }

    // 4. Bud under 50 % avvises, bud på nøyaktig 50 % godtas
    [Fact]
    public async Task Offer_50PercentLimit()
    {
        var listing = _listings.Add("selger", 1000);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(listing.Id, "kjoper", new CreateOfferDto { Amount = 499 }));

        var offer = await _service.CreateAsync(listing.Id, "kjoper", new CreateOfferDto { Amount = 500 });
        Assert.Equal(OfferStatus.Pending, offer.Status);
    }

    // 5. Aksept gjør annonsen Sold og avslår andre Pending-bud
    [Fact]
    public async Task AcceptOffer_SellsListing_AndRejectsOthers()
    {
        var listing = _listings.Add("selger", 1000);
        var accepted = _offers.Add(listing.Id, "kjoper", 900);
        var other = _offers.Add(listing.Id, "annen", 800);

        await _service.AcceptAsync(accepted.Id);

        Assert.Equal(ListingStatus.Sold, listing.Status);
        Assert.Equal(900, listing.SoldPrice);
        Assert.NotNull(listing.SoldDate);
        Assert.Equal(OfferStatus.Accepted, accepted.Status);
        Assert.Equal(OfferStatus.Rejected, other.Status);
    }
}
