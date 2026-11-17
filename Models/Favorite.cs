namespace Mappeinnlevering_1.Models;

public class Favorite
{
    public int Id { get; set; }
    
    public int ListingId { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    
    public Listing? Listing {  get; set; }
    
    public ApplicationUser? Buyer { get; set; }
}