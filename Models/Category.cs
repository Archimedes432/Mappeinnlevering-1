using System.ComponentModel.DataAnnotations;

namespace Mappeinnlevering_1.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public List<Listing> Listings { get; set; } = new();
}