using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Repositories.Interfaces;

public interface IListingRepository
{
    Task<List<Listing>> GetAllAsync();
    
    Task<Listing?> GetByIdAsync(int id);
    
    Task AddAsync(Listing listing);
    
    void Update(Listing listing);
    
    Task SaveChangesAsync();
}