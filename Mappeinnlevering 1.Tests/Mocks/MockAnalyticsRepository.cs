using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;

namespace Mappeinnlevering_1.Tests.Mocks;

// Testdata
public class MockAnalyticsRepository : IAnalyticsRepository
{
    public List<Category> Categories = new List<Category>
    {
      new Category {Id = 1, Name = "Elektronikk"}
    };

    
    public List<Listing> Listings = new List<Listing>();

    private int _nextId = 1;

    public Task<List<Listing>> GetSoldListingsAsync(int categoryId)
    {
        List<Listing> result = Listings
            .Where(l => l.CategoryId == categoryId && l.Status == ListingStatus.Sold && l.SoldPrice != null)
            .ToList();
        return Task.FromResult(result);
    }

    public Task<List<Listing>> GetListingsInCategoryAsync(int categoryId)
    {
        List<Listing> result = Listings.Where(l => l.CategoryId == categoryId).ToList();
        return Task.FromResult(result);
    }

    public Task<Listing?> GetListingAsync(int listingId)
    {
        Listing? result = Listings.FirstOrDefault(l => l.Id == listingId);
        return Task.FromResult(result);
    }

    public Task<Category?> GetCategoryAsync(int categoryId)
    {
        Category? result = Categories.FirstOrDefault(c => c.Id == categoryId);
        return Task.FromResult(result);
    }

    public Task<List<Category>> GetCategoriesAsync()
    {
        return Task.FromResult(Categories.ToList());
    }

    // Opprettet 1. jan, solgt 11. jan (10 dager)
    public Listing AddSold(int soldPrice, Condition condition, int price)
    {
        Listing l = new Listing
        {
            Id = _nextId++,
            CategoryId = 1,
            Title = "Solgt",
            Price = price,
            SoldPrice = soldPrice,
            Condition = condition,
            Status = ListingStatus.Sold,
            CreatedDate = new DateTime(2026, 1, 1),
            SoldDate = new DateTime(2026, 1, 11)
        };
        Listings.Add(l);
        return l;
    }

    public Listing AddSold(int soldPrice, Condition condition)
    {
        return AddSold(soldPrice, condition, soldPrice);
    }

    public Listing AddActive(int price, Condition condition)
    {
        Listing l = new Listing
        {
            Id = _nextId++,
            CategoryId = 1,
            Title = "Aktiv",
            Price = price,
            Condition = condition,
            Status = ListingStatus.Active,
            CreatedDate = new DateTime(2026, 2, 1)
        };
        Listings.Add(l);
        return l;
    }
}


