using Mappeinnlevering_1.Models.Enums;

namespace Mappeinnlevering_1.Models;

public class Offer
{
    public int Id { get; set; }
    
    public int ListingId { get; set; }
    
    public string BuyerId { get; set; } = string.Empty;
    
    public int Amount { get; set; }
    
    public OfferStatus Status { get; set; }
    
    public DateTime CreatedDate { get; set; }
}