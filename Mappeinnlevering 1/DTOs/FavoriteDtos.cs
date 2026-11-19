using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Models.Enums;

namespace Mappeinnlevering_1.DTOs;

public class FavoriteDto
{
    public int ListingId { get; set; }
    public string Title { get; set; } = "";
    public int Price { get; set; }
    public Condition Condition { get; set; }
    public ListingStatus Status { get; set; }
    public string? ImageUrl { get; set; }
}