using Microsoft.AspNetCore.Identity;

namespace Mappeinnlevering_1.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}