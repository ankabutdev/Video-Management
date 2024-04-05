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

    public async Task<bool> CreateAsync(Product entity)
    {
        try
        {
            await _context.Products.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int Id)
    {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
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
            p.Name.Contains(query) ||
            p.Description.Contains(query));
    }

    public async Task<bool> UpdateAsync(Product entity)
    {
        try
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
