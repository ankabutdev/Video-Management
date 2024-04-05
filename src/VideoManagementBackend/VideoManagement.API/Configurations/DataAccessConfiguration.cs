using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VideoManagement.API.Contexts;

namespace VideoManagement.API.Configurations;

public static class DataAccessConfiguration
{
    public static IServiceCollection ConfigureDataAccess(this IServiceCollection services,
    IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(defaultConnection);

        });

        services.AddControllersWithViews()
            .AddJsonOptions(x => x.JsonSerializerOptions
            .ReferenceHandler = ReferenceHandler.IgnoreCycles);

        return services;
    }
}
