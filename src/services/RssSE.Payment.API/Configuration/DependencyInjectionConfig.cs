using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Payment.API.Data.Context;
using RssSE.Payment.API.Data.Repositories;
using RssSE.Payment.API.Facade;
using RssSE.Payment.API.Models.Interfaces;
using RssSE.Payment.API.Services;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Payment.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPaymentFacade, PaymentFacade>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<PaymentDbContext>();
        }
    }
}
