using Microsoft.Extensions.DependencyInjection;
using RssSE.Catalog.API.Data;
using RssSE.Catalog.API.Data.Repositories;
using RssSE.Catalog.API.Models.Repositories;

namespace RssSE.Catalog.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();
        }
    }
}
