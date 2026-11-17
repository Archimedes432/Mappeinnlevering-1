using Mappeinnlevering_1.Models;

namespace Mappeinnlevering_1.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    
    Task AddAsync(Category category);
    
    void Update(Category category);
    
    void Delete(Category category);
    
    Task<bool> HasListingsAsync(int categoryId);
    
    Task SaveChangesAsync();
}