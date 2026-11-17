using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Repositories.Interfaces;

public interface IOfferRepository
{
    Task<List<Offer>> GetAllAsync();

    Task<Offer?> GetByIdAsync(int id);

    Task<List<Offer>> GetByListingIdAsync(int listingId);

    Task AddAsync(Offer offer);

    void Update(Offer offer);

    Task SaveChangesAsync();
}