using Mappeinnlevering_1.Data;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mappeinnlevering_1.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly ApplicationDbContext _db;

    public FavoriteRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Listing>> GetFavoriteListingsAsync(string userId)
    {
        // Finn id-ene til annonsene brukern har som favoritt
        List<int> listingIds = await _db.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.ListingId)
            .ToListAsync();

        // Hent annonsenen
        return await _db.Listings
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync();
    }

    public async Task<Favorite?> GetFavoriteAsync(string userId, int listingId)
    {
        return await _db.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ListingId == listingId);
    }

    public async Task<bool> ListingExistsAsync(int listingId)
    {
        return await _db.Listings.AnyAsync(l => l.Id == listingId);
    }

    public async Task AddAsync(Favorite favorite)
    {
        _db.Favorites.Add(favorite);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(Favorite favorite)
    {
        _db.Favorites.Remove(favorite);
        await _db.SaveChangesAsync();
    }
}