using VideoManagement.API.Dtos;
using VideoManagement.API.Entities;
using VideoManagement.API.Repository;

namespace VideoManagement.API.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> CreateAsync(ProductCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<Product>> SearchAsync(string query)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int Id, ProductUpdateDto dto)
    {
        throw new NotImplementedException();
    }
}
