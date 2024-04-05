using VideoManagement.API.Entities;

namespace VideoManagement.API.Repository;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<bool> CreateAsync(Product entity);

    Task<bool> UpdateAsync(Product entity);

    Task<bool> DeleteAsync(int Id);

    Task<IQueryable<Product>> SearchAsync(string query);

}
