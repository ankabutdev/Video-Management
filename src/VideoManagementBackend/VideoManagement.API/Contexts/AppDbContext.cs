using Microsoft.EntityFrameworkCore;
using VideoManagement.API.Entities;

namespace VideoManagement.API.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Product> Products { get; set; }
}
