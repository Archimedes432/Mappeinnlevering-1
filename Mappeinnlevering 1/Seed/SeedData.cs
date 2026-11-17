using Mappeinnlevering_1.Models;
using Microsoft.AspNetCore.Identity;
using Mappeinnlevering_1.Data;
using Mappeinnlevering_1.Models.Enums;

namespace Mappeinnlevering_1.Seed;

public static class SeedData
{
    // Seed Roles
    public static async Task SeedUsersAndRolesAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(
                new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(
                new IdentityRole("User"));
        }

        await CreateUserAsync(
            userManager,
            "admin@brukt.no",
            "Admin Adminsen",
            "Admin123!",
            "Admin");

        await CreateUserAsync(
            userManager,
            "ola@brukt.no",
            "Ola Nordmann",
            "User123!",
            "User");

        await CreateUserAsync(
            userManager,
            "kari@brukt.no",
            "Kari Nordmann",
            "User123!",
            "User");

        await CreateUserAsync(
            userManager,
            "per@brukt.no",
            "Per Hansen",
            "User123!",
            "User");
    }
    
    // Seed Users
    private static async Task CreateUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string fullName,
        string password,
        string role)
    {
        var existingUser =
            await userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return;

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName
        };

        await userManager.CreateAsync(
            user,
            password);

        await userManager.AddToRoleAsync(
            user,
            role);
    }

    // Seed Chategory
    public static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        if (context.Categories.Any())
            return;

        var categories = new List<Category>
        {
            new() { Name = "Elektronikk" },
            new() { Name = "Sykkel" },
            new() { Name = "Møbler" },
            new() { Name = "Friluft" }
        };
        
        context.Categories.AddRange(categories);
        
        await context.SaveChangesAsync();
    }

    // Seed Listings
    public static async Task SeedListingAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        if (context.Listings.Any())
            return;

        var users = userManager.Users.ToList();

        var categories = context.Categories.ToList();

        if (!users.Any() || !categories.Any())
            return;

        var random = new Random();

        var listings = new List<Listing>();

        for (int i = 1; i <= 40; i++)
        {
            // hardcoded category distribution
            Category category;
            if (i <= 10)
            {
                category = categories[0]; // Electronics
            }
            else if (i <= 17)
            {
                category = categories[1]; // Bike
            }
            else if (i <= 19)
            {
                category = categories[2]; // Furnature
            }
            else if (i == 20)
            {
                category = categories[3]; // Outdoor
            }
            else
            {
                category = categories[random.Next(categories.Count)];
            }

            var seller = users[random.Next(users.Count)];

            bool sold = i <= 20;

            listings.Add(new Listing()
            {
                Title = $"Vare {i}",
                Description = $"Beskrivelse avcvare {i}",

                Price = random.Next(100, 1500),
                Condition = (Condition)random.Next(0, 4),

                Status = sold
                    ? ListingStatus.Sold
                    : ListingStatus.Active,

                CreatedDate =
                    DateTime.UtcNow.AddDays(-random.Next(1, 100)),

                SoldDate = sold
                    ? DateTime.UtcNow.AddDays(
                        -random.Next(1, 30))
                    : null,
                
                SoldPrice = sold
                    ? random.Next(100, 1500)
                    : null,

                CategoryId = category.Id,

                SellerId = seller.Id
            });
        }
        
        // Overpriced Item
        listings.Add(new Listing
        {
            Title = "Overpriset sykkel",
            Description = "Testannonse",
            Price = 500000,
            Condition = Condition.Used,
            Status = ListingStatus.Active,
            CreatedDate = DateTime.UtcNow,
            CategoryId = categories[1].Id,
            SellerId = users[0].Id
        });
        
        context.Listings.AddRange(listings);
        
        await context.SaveChangesAsync();
    }
    
    // Seed Price Change
    public static async Task SeedPriceChangesAsync(
        ApplicationDbContext context)
    {
        if (context.PriceChanges.Any())
            return;

        var random = new Random();

        var listings = context.Listings
            .Where(l => l.Status == ListingStatus.Sold)
            .Take(15)
            .ToList();

        var changes = new List<PriceChange>();

        foreach (var listing in listings)
        {
            var oldPrice = listing.Price + random.Next(100, 1000);

            changes.Add(new PriceChange
            {
                ListingId = listing.Id,
                OldPrice = oldPrice,
                NewPrice = listing.Price,
                ChangedDate = listing.CreatedDate.AddDays(2)
            });
        }

        context.PriceChanges.AddRange(changes);

        await context.SaveChangesAsync();
    }
    
    // Seed Offers
    public static async Task SeedOffersAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        if (context.Offers.Any())
            return;

        var users = userManager.Users.ToList();

        var listings = context.Listings.ToList();

        var random = new Random();

        var offers = new List<Offer>();

        foreach (var listing in listings.Take(20))
        {
            var buyers = users
                .Where(u => u.Id != listing.SellerId)
                .ToList();
            
            if (!buyers.Any())
                continue;
            
            var buyer = buyers[random.Next(buyers.Count)];

            offers.Add(new Offer
            {
                ListingId = listing.Id,
                BuyerId = buyer.Id,
                Amount = (int)(listing.Price * 0.8),
                Status = OfferStatus.Pending,
                CreatedDate = DateTime.UtcNow.AddDays(-2)
            });
        }

        foreach (var listing in listings.Skip(20).Take(5))
        {
            var buyer = users.First(u =>  u.Id != listing.SellerId);

            offers.Add(new Offer
            {
                ListingId = listing.Id,
                BuyerId = buyer.Id,
                Amount = listing.Price,
                Status = OfferStatus.Accepted,
                CreatedDate = DateTime.UtcNow.AddDays(-5)
            });
        }
        
        foreach (var listing in listings.Skip(25).Take(5))
        {
            var buyer = users.First(u => u.Id != listing.SellerId);

            offers.Add(new Offer
            {
                ListingId = listing.Id,
                BuyerId = buyer.Id,
                Amount = (int)(listing.Price * 0.75),
                Status = OfferStatus.Rejected,
                CreatedDate = DateTime.UtcNow.AddDays(-4)
            });
        }

        foreach (var listing in listings.Skip(30).Take(5))
        {
            var buyer = users.First(u => u.Id != listing.SellerId);

            offers.Add(new Offer
            {
                ListingId = listing.Id,
                BuyerId = buyer.Id,
                Amount = (int)(listing.Price * 0.7),
                Status = OfferStatus.Withdrawn,
                CreatedDate = DateTime.UtcNow.AddDays(-3)
            });
        }
        
        context.Offers.AddRange(offers);
        
        await context.SaveChangesAsync();
    }
    
    // Seed Favorites
    public static async Task SeedFavoritesAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        if (context.Favorites.Any())
            return;

        var users = userManager.Users.ToList();
        
        var listings = context.Listings.Take(15).ToList();
        
        var favorites = new List<Favorite>();

        foreach (var listing in listings)
        {
            foreach (var user in users.Where(u => u.Id != listing.SellerId).Take(2))
            {
                favorites.Add(new Favorite
                {
                    ListingId = listing.Id,
                    UserId = user.Id,
                });
            }
        }
        
        context.Favorites.AddRange(favorites);
        
        await context.SaveChangesAsync();
    }
}