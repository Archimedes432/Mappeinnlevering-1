using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Repositories.Interfaces;

public interface IAnalyticsRepository
{
    Task<List<Listing>> GetSoldListingsAsync(int categoryId);
    Task<List<Listing>> GetListingsInCategoryAsync(int categoryId);
    Task<Listing?> GetListingAsync(int listingId);
    Task<Category?> GetCategoryAsync(int categoryId);
    Task<List<Category>> GetCategoriesAsync();
}