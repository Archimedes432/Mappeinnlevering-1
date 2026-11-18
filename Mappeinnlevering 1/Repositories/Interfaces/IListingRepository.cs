using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Repositories.Interfaces;

public interface IListingRepository
{
    Task<List<Listing>> GetAllAsync();

    Task<List<Listing>> GetMyListingsAsync(string sellerId);

    Task<Listing?> GetByIdAsync(int id);

    Task AddAsync(Listing listing);

    Task AddPriceChangeAsync(PriceChange priceChange);

    void Update(Listing listing);

    Task SaveChangesAsync();
}