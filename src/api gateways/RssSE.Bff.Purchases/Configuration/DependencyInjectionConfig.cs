using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Services;
using RssSE.Bff.Purchases.Services.Interfaces;
using RssSE.WebApi.Core.Extensions;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Bff.Purchases.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.RetryAsyncWithThreeAttemptsAndLogging())
              .AddPolicyHandler(PollyExtensions.CircuitBreakAfterThreeAttempts());

            services.AddHttpClient<ICartService, CartService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.RetryAsyncWithThreeAttemptsAndLogging())
              .AddPolicyHandler(PollyExtensions.CircuitBreakAfterThreeAttempts());

            services.AddHttpClient<IOrderService, OrderService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.RetryAsyncWithThreeAttemptsAndLogging())
             .AddPolicyHandler(PollyExtensions.CircuitBreakAfterThreeAttempts());
        }
    }
}
