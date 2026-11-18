using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Repositories.Interfaces;
using Mappeinnlevering_1.Services.Interfaces;

namespace Mappeinnlevering_1.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IListingRepository _listingRepository;

    public OfferService(
        IOfferRepository offerRepository,
        IListingRepository listingRepository)
    {
        _offerRepository = offerRepository;
        _listingRepository = listingRepository;
    }

    public async Task<List<OfferDtos>> GetAllAsync()
    {
        var offers = await _offerRepository.GetAllAsync();

        return offers.Select(o => new OfferDtos
        {
            Id = o.Id,
            ListingId = o.ListingId,
            BuyerId = o.BuyerId,
            Amount = o.Amount,
            Status = o.Status,
            CreatedDate = o.CreatedDate
        }).ToList();
    }

    public async Task<OfferDtos?> GetByIdAsync(int id)
    {
        var offer = await _offerRepository.GetByIdAsync(id);

        if (offer == null)
            return null;

        return new OfferDtos
        {
            Id = offer.Id,
            ListingId = offer.ListingId,
            BuyerId = offer.BuyerId,
            Amount = offer.Amount,
            Status = offer.Status,
            CreatedDate = offer.CreatedDate
        };
    }

    public async Task<OfferDtos> CreateAsync(
        int listingId,
        string buyerId,
        CreateOfferDto dto)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId);

        if (listing == null)
            throw new Exception("Listing not found");

        if (listing.SellerId == buyerId)
            throw new InvalidOperationException(
                "Cannot make offer on your own listing.");

        if (listing.Status != ListingStatus.Active)
            throw new InvalidOperationException(
                "Listing must be active.");

        if (dto.Amount < listing.Price * 0.5)
            throw new InvalidOperationException(
                "Offer must be at least 50% of asking price.");

        var existingOffer =
            await _offerRepository.GetPendingOfferAsync(
                listingId,
                buyerId);

        if (existingOffer != null)
        {
            throw new InvalidOperationException(
                "You alredy have a pending offer for this listing.");
        }

        var offer = new Offer
        {
            ListingId = listingId,
            BuyerId = buyerId,
            Amount = dto.Amount,
            Status = OfferStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        await _offerRepository.AddAsync(offer);
        await _offerRepository.SaveChangesAsync();

        return new OfferDtos
        {
            Id = offer.Id,
            ListingId = offer.ListingId,
            BuyerId = offer.BuyerId,
            Amount = offer.Amount,
            Status = offer.Status,
            CreatedDate = offer.CreatedDate
        };
    }

    public async Task<bool> WithdrawAsync(int id)
    {
        var offer = await _offerRepository.GetByIdAsync(id);

        if (offer == null)
            return false;

        if (offer.Status != OfferStatus.Pending)
            throw new InvalidOperationException(
                "Only pending offers can be withdrawn.");

        offer.Status = OfferStatus.Withdrawn;

        _offerRepository.Update(offer);

        await _offerRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AcceptAsync(int id)
    {
        var offer = await _offerRepository.GetByIdAsync(id);

        if (offer == null)
            return false;

        if (offer.Status != OfferStatus.Pending)
            throw new InvalidOperationException(
                "Only pending offers can be accepted.");

        var listing =
            await _listingRepository.GetByIdAsync(
                offer.ListingId);

        if (listing == null)
            return false;

        offer.Status = OfferStatus.Accepted;

        listing.Status = ListingStatus.Sold;
        listing.SoldDate = DateTime.UtcNow;
        listing.SoldPrice = offer.Amount;

        _offerRepository.Update(offer);

        _listingRepository.Update(listing);

        var otherOffers =
            await _offerRepository.GetByListingIdAsync(
                offer.ListingId);

        foreach (var otherOffer in otherOffers)
        {
            if (otherOffer.Id != offer.Id &&
                otherOffer.Status == OfferStatus.Pending)
            {
                otherOffer.Status =
                    OfferStatus.Rejected;

                _offerRepository.Update(otherOffer);
            }
        }

        await _offerRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RejectAsync(int id)
    {
        var offer = await _offerRepository.GetByIdAsync(id);

        if (offer == null)
            return false;

        if (offer.Status != OfferStatus.Pending)
            throw new InvalidOperationException(
                "Only pending offers can be rejected.");

        offer.Status = OfferStatus.Rejected;

        _offerRepository.Update(offer);

        await _offerRepository.SaveChangesAsync();

        return true;
    }
}