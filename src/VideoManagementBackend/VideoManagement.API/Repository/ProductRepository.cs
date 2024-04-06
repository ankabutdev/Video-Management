using Microsoft.EntityFrameworkCore;
using VideoManagement.API.Contexts;
using VideoManagement.API.Entities;

namespace VideoManagement.API.Repository;

#pragma warning disable

public class ProductRepository : IProductRepository
{

    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Product entity)
    {
        try
        {
            await _context.Products.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> DeleteAsync(Product product)
    {
        try
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _context.Products.FindAsync(id);

    public async Task<IQueryable<Product>> SearchAsync(string query)
    {
        return _context.Products
            .Where(p =>
            p.Name.ToLower().Contains(query) ||
            p.Description.ToLower().Contains(query));
    }

    public async Task<int> UpdateAsync(Product entity)
    {
        try
        {
            _context.Products.Update(entity);
            return await _context.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }
}
