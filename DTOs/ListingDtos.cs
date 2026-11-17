using Mappeinnlevering_1.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mappeinnlevering_1.DTOs;

public class ListingDtos
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Price { get; set; }

    public Condition Condition { get; set; }

    public ListingStatus Status { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? SoldDate { get; set; }

    public int? SoldPrice { get; set; }

    public int CategoryId { get; set; }

    public string SellerId { get; set; } = string.Empty;
}

public class CreateListingDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Price { get; set; }
    
    public Condition Condition { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public int CategoryId { get; set; }
}

public class UpdateListingDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Price { get; set; }
    public Condition Condition { get; set; }
    public string? ImageUrl { get; set; }
}