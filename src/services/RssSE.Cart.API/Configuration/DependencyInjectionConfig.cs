using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Cart.API.Data;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Cart.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<CartDbContext>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}
