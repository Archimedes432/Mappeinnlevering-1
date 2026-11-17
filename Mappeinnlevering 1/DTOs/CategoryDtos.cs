using System.ComponentModel.DataAnnotations;

namespace Mappeinnlevering_1.DTOs;

public class CategoryDtos
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class CreateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}

public class UpdateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}