using Mappeinnlevering_1.Data;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mappeinnlevering_1.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly ApplicationDbContext _context;

    public OfferRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Offer>> GetAllAsync()
    {
        return await _context.Offers.ToListAsync();
    }

    public async Task<Offer?> GetByIdAsync(int id)
    {
        return await _context.Offers
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Offer>> GetByListingIdAsync(int listingId)
    {
        return await _context.Offers
            .Where(o => o.ListingId == listingId)
            .ToListAsync();
    }

    public async Task<Offer?> GetPendingOfferAsync(
        int listingId,
        string buyerId)
    {
        return await _context.Offers
            .FirstOrDefaultAsync(o =>
                o.ListingId == listingId &&
                o.BuyerId == buyerId &&
                o.Status == OfferStatus.Pending);
    }

    public async Task AddAsync(Offer offer)
    {
        await _context.Offers.AddAsync(offer);
    }

    public void Update(Offer offer)
    {
        _context.Offers.Update(offer);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}