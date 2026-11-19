using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Repositories.Interfaces;
using Mappeinnlevering_1.Services.Interfaces;

namespace Mappeinnlevering_1.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _repo;

    public FavoriteService(IFavoriteRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<FavoriteDto>> GetFavoritesAsync(string userId)
    {
        List<Listing> listings = await _repo.GetFavoriteListingsAsync(userId);

        List<FavoriteDto> result = new List<FavoriteDto>();
        foreach (Listing l in listings)
        {
            FavoriteDto dto = new FavoriteDto();
            dto.ListingId = l.Id;
            dto.Title = l.Title;
            dto.Price = l.Price;
            dto.Condition = l.Condition;
            dto.Status = l.Status;
            dto.ImageUrl = l.ImageUrl;
            result.Add(dto);
        }

        return result;
    }

    public async Task<FavoriteResult> AddFavoriteAsync(string userId, int listingId)
    {
        if (!await _repo.ListingExistsAsync(listingId))
        {
            return FavoriteResult.ListingNotFound;
        }

        // Samme annonse kan ikke legges til to ganger
        Favorite? existing = await _repo.GetFavoriteAsync(userId, listingId);
        if (existing != null)
        {
            return FavoriteResult.AlreadyFavorite;
        }

        Favorite favorite = new Favorite();
        favorite.UserId = userId;
        favorite.ListingId = listingId;
        await _repo.AddAsync(favorite);

        return FavoriteResult.Ok;
    }

    public async Task<FavoriteResult> RemoveFavoriteAsync(string userId, int listingId)
    {
        Favorite? existing = await _repo.GetFavoriteAsync(userId, listingId);
        if (existing == null)
        {
            return FavoriteResult.NotFavorite;
        }

        await _repo.RemoveAsync(existing);
        return FavoriteResult.Ok;
    }
}