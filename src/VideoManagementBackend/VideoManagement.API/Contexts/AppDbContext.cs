using Microsoft.EntityFrameworkCore;
using VideoManagement.API.Entities;

namespace VideoManagement.API.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Name).IsRequired().HasMaxLength(255);
            entity.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            entity.Property(x => x.VideoUrl).IsRequired();
            
            entity.Property(x => x.SortNumber).UseIdentityByDefaultColumn();
            entity.HasIndex(x => x.SortNumber).IsUnique();
        });
    }
}
