using VideoManagement.API.Repository;
using VideoManagement.API.Services;

namespace VideoManagement.API.Configurations;

public static class CustomService
{
    public static void AddCustomService(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IVideoService, VideoService>();
    }
}
