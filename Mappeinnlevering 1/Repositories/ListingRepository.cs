using Mappeinnlevering_1.Data;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mappeinnlevering_1.Repositories;

public class ListingRepository : IListingRepository
{
    private readonly ApplicationDbContext _context;

    public ListingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Listing>> GetAllAsync()
    {
        return await _context.Listings
            .Where(l => l.Status == ListingStatus.Active)
            .ToListAsync();
    }

    public async Task<List<Listing>> GetMyListingsAsync(
        string sellerId)
    {
        return await _context.Listings
            .Where(l => l.SellerId == sellerId)
            .ToListAsync();
    }

    public async Task<Listing?> GetByIdAsync(int id)
    {
        return await _context.Listings
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddAsync(Listing listing)
    {
        await _context.Listings.AddAsync(listing);
    }

    public async Task AddPriceChangeAsync(
        PriceChange priceChange)
    {
        await _context.PriceChanges.AddAsync(priceChange);
    }

    public void Update(Listing listing)
    {
        _context.Listings.Update(listing);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}