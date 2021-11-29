using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Core.Mediator;
using RssSE.Order.API.Application.Queries;
using RssSE.Order.Domain.Repositories;
using RssSE.Order.Infra.Data.Context;
using RssSE.Order.Infra.Data.Repositories;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Order.API.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQuery, VoucherQuery>();

            //Data
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<OrdersDbContext>();
        }
    }
}
