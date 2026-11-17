using Mappeinnlevering_1.DTOs;

namespace Mappeinnlevering_1.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDtos>> GetAllAsync();

    Task<CategoryDtos?> GetByIdAsync(int id);

    Task<CategoryDtos> CreateAsync(CreateCategoryDto dto);

    Task<bool> UpdateAsync(int id, UpdateCategoryDto dto);

    Task<bool> DeleteAsync(int id);
}