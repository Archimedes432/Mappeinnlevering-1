using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;

namespace Mappeinnlevering_1.Tests.Mocks;

// Falsk repository som lagrer annonser i en liste
public class MockListingRepository : IListingRepository
{
    public List<Listing> Listings = new List<Listing>();
    public List<PriceChange> PriceChanges = new List<PriceChange>();

    public Task<List<Listing>> GetAllAsync() => Task.FromResult(Listings);

    public Task<List<Listing>> GetMyListingsAsync(string sellerId) =>
        Task.FromResult(Listings.Where(l => l.SellerId == sellerId).ToList());

    public Task<Listing?> GetByIdAsync(int id) =>
        Task.FromResult(Listings.FirstOrDefault(l => l.Id == id));

    public Task AddAsync(Listing listing)
    {
        Listings.Add(listing);
        return Task.CompletedTask;
    }

    public Task AddPriceChangeAsync(PriceChange priceChange)
    {
        PriceChanges.Add(priceChange);
        return Task.CompletedTask;
    }

    public void Update(Listing listing) { }

    public Task SaveChangesAsync() => Task.CompletedTask;

    // Hjelpemetode for testene
    public Listing Add(string sellerId, int price, ListingStatus status = ListingStatus.Active)
    {
        var listing = new Listing { Id = Listings.Count + 1, SellerId = sellerId, Price = price, Status = status };
        Listings.Add(listing);
        return listing;
    }
}
