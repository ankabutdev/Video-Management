using VideoManagement.API.Entities;

namespace VideoManagement.API.Repository;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<int> CreateAsync(Product entity);

    Task<int> UpdateAsync(Product entity);

    Task<int> DeleteAsync(Product product);

    Task<IQueryable<Product>> SearchAsync(string query);

    Task<Product?> GetVideoUrlBySortNumberAsync(int sortNumber);

}
