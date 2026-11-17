using Mappeinnlevering_1.Data;
using Mappeinnlevering_1.Models;
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

    public void Update(Listing listing)
    {
        _context.Listings.Update(listing);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}