using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Repositories.Interfaces;

public interface IFavoriteRepository
{
    Task<List<Listing>> GetFavoriteListingsAsync(string userId);
    Task<Favorite?> GetFavoriteAsync(string userId, int listingId);
    Task<bool> ListingExistsAsync(int listingId);
    Task AddAsync(Favorite favorite);
    Task RemoveAsync(Favorite favorite);
}