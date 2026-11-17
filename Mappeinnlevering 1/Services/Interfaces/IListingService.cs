using Mappeinnlevering_1.DTOs;

namespace Mappeinnlevering_1.Services.Interfaces;

public interface IListingService
{
    Task<List<ListingDtos>> GetAllAsync();
    
    Task<ListingDtos?> GetByIdAsync(int id);
    
    Task<ListingDtos> CreateAsync(
        CreateListingDto dto,
        string sellerId);

    Task<bool> UpdateAsync(
        int id,
        UpdateListingDto dto);
    
    Task<bool> DeleteAsync(int id);
}