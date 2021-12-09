using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDevPack.Security.Jwt.AspNetCore;
using RssSE.Identity.API.Helpers;
using RssSE.WebApi.Core.Identity;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Identity.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddCompressionConfiguration();

            services.AddControllers();

            services.AddScoped<IAspNetUser, AspNetUser>();

            //Helper
            services.AddScoped<IUserLoginHelper, UserLoginHelper>();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseRouting();

            //deve sempre estar entre routing e endpoints
            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseJwksDiscovery();

            return app;
        }
    }
}
