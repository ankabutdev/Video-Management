using VideoManagement.API.Dtos;
using VideoManagement.API.Entities;

namespace VideoManagement.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product> GetByIdAsync(int id);

    Task<bool> CreateAsync(ProductCreateDto dto);

    Task<bool> UpdateAsync(int Id, ProductUpdateDto dto);

    Task<bool> DeleteAsync(int Id);

    Task<IQueryable<Product>> SearchAsync(string query);

}
