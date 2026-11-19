using Mappeinnlevering_1.DTOs;

namespace Mappeinnlevering_1.Services.Interfaces;


public enum FavoriteResult
{
    Ok,
    ListingNotFound,
    AlreadyFavorite,
    NotFavorite
}

public interface IFavoriteService
{
    Task<List<FavoriteDto>> GetFavoritesAsync(string userId);
    Task<FavoriteResult> AddFavoriteAsync(string userId, int listingId);
    Task<FavoriteResult> RemoveFavoriteAsync(string userId, int listingId);
}