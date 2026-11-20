using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;

namespace Mappeinnlevering_1.Tests.Mocks;

// Falsk repository som lagrer bud i en liste
public class MockOfferRepository : IOfferRepository
{
    public List<Offer> Offers = new List<Offer>();

    public Task<List<Offer>> GetAllAsync() => Task.FromResult(Offers);

    public Task<Offer?> GetByIdAsync(int id) =>
        Task.FromResult(Offers.FirstOrDefault(o => o.Id == id));

    public Task<Offer?> GetPendingOfferAsync(int listingId, string buyerId) =>
        Task.FromResult(Offers.FirstOrDefault(o =>
            o.ListingId == listingId && o.BuyerId == buyerId && o.Status == OfferStatus.Pending));

    public Task<List<Offer>> GetByListingIdAsync(int listingId) =>
        Task.FromResult(Offers.Where(o => o.ListingId == listingId).ToList());

    public Task AddAsync(Offer offer)
    {
        offer.Id = Offers.Count + 1;
        Offers.Add(offer);
        return Task.CompletedTask;
    }

    public void Update(Offer offer) { }

    public Task SaveChangesAsync() => Task.CompletedTask;

    // Hjelpemetode for testene
    public Offer Add(int listingId, string buyerId, int amount)
    {
        var offer = new Offer { Id = Offers.Count + 1, ListingId = listingId, BuyerId = buyerId, Amount = amount, Status = OfferStatus.Pending };
        Offers.Add(offer);
        return offer;
    }
}
