namespace Mappeinnlevering_1.Models;

public class PriceChange
{
    public int Id { get; set; }
    
    public int ListingId { get; set; }
    
    public int OldPrice { get; set; }
    
    public int NewPrice { get; set; }
    
    public DateTime ChangedDate { get; set; }
}