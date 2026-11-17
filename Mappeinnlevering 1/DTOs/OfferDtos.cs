using Mappeinnlevering_1.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mappeinnlevering_1.DTOs;

public class OfferDtos
{
    public int Id { get; set; }
    
    public int ListingId { get; set; }
    
    public string BuyerId { get; set; } = string.Empty;
    
    public int Amount { get; set; }
    
    public OfferStatus Status { get; set; }
    
    public DateTime CreatedDate { get; set; }
}

public class CreateOfferDto
{
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }
}