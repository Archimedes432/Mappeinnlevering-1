using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Services.Interfaces;

public interface IOfferService
{
    Task<List<OfferDtos>> GetAllAsync();
    
    Task<OfferDtos?> GetByIdAsync(int id);

    Task<OfferDtos> CreateAsync(
        int listingId,
        string buyerId,
        CreateOfferDto dto);

    Task<bool> WithdrawAsync(int id);
    
    Task<bool> AcceptAsync(int id);
    
    Task<bool> RejectAsync(int id);
}