using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.Jwt;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;
using RssSE.Identity.API.Extensions;
using RssSE.Identity.Data.Context;
using RssSE.WebApi.Core.Identity;

namespace RssSE.Identity.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddJwksManager(options => options.Jws = JwsAlgorithm.ES256)
                .PersistKeysToDatabaseStore<ApplicationDbContext>();

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityPortugueseMessages>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
