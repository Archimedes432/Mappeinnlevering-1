using Microsoft.AspNetCore.Identity;

namespace Mappeinnlevering_1.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public List<Listing> Listings { get; set; } = new();

    public List<Offer> Offers { get; set; } = new();

    public List<Favorite> Favorites { get; set; } = new();
}