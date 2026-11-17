using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Mappeinnlevering_1.Services.Interfaces;

namespace Mappeinnlevering_1.Services;

public class ListingService : IListingService
{
    private readonly IListingRepository _repository;

    public ListingService(IListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ListingDtos>> GetAllAsync()
    {
        var listings = await _repository.GetAllAsync();

        return listings.Select(l => new ListingDtos
        {
            Id = l.Id,
            Title = l.Title,
            Description = l.Description,
            Price = l.Price,
            Condition = l.Condition,
            Status = l.Status,
            ImageUrl = l.ImageUrl,
            CreateDate = l.CreatedDate,
            SoldDate = l.SoldDate,
            SoldPrice = l.SoldPrice,
            CategoryId = l.CategoryId,
            SellerId = l.SellerId
        }).ToList();
    }

    public async Task<ListingDtos?> GetByIdAsync(int id)
    {
        var listing = await _repository.GetByIdAsync(id);

        if (listing == null)
            return null;

        return new ListingDtos
        {
            Id = listing.Id,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Condition = listing.Condition,
            Status = listing.Status,
            ImageUrl = listing.ImageUrl,
            CreateDate = listing.CreatedDate,
            SoldDate = listing.SoldDate,
            SoldPrice = listing.SoldPrice,
            CategoryId = listing.CategoryId,
            SellerId = listing.SellerId
        };
    }

    public async Task<ListingDtos> CreateAsync(
        CreateListingDto dto,
        string sellerId)
    {
        var listing = new Listing
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Condition = dto.Condition,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            SellerId = sellerId,
            CreatedDate = DateTime.UtcNow,
            Status = ListingStatus.Active
        };

        await _repository.AddAsync(listing);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(listing.Id)
               ?? throw new Exception("Listing could not be created");
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateListingDto dto)
    {
        var listing = await _repository.GetByIdAsync(id);

        if (listing == null)
            return false;

        if (listing.Status == ListingStatus.Sold)
            throw new InvalidOperationException(
                "Sold listings cannot be modified.");

        listing.Title = dto.Title;
        listing.Description = dto.Description;
        listing.Condition = dto.Condition;
        listing.ImageUrl = dto.ImageUrl;

        listing.Price = dto.Price;

        _repository.Update(listing);

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var listing = await _repository.GetByIdAsync(id);

        if (listing == null)
            return false;

        listing.Status = ListingStatus.Removed;

        _repository.Update(listing);

        await _repository.SaveChangesAsync();

        return true;
    }
}