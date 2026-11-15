using Mappeinnlevering_1.Data;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mappeinnlevering_1.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly ApplicationDbContext _db;

    public AnalyticsRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<Listing>> GetSoldListingsAsync(int categoryId)
    {
        return await _db.Listings
            .Where(l => l.CategoryId == categoryId
                        && l.Status == ListingStatus.Sold
                        && l.SoldPrice != null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Listing>> GetListingsInCategoryAsync(int categoryId)
    {
        return await _db.Listings
            .Where(l => l.CategoryId == categoryId)
            .Include(l => l.PriceChanges)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Listing?> GetListingAsync(int listingId)
    {
        return await _db.Listings
            .Include(l => l.PriceChanges)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == listingId);
    }

    public async Task<Category?> GetCategoryAsync(int categoryId)
    {
        return await _db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId)
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}