using Microsoft.AspNetCore.Identity;

namespace Mappeinnlevering_1.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public List<Listing>? Listing { get; set; } = new();

    public List<Offer>? Offer { get; set; } = new();

    public List<Favorite> Favorites { get; set; } = new();
}