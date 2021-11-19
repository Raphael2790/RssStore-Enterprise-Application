using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RssSE.WebApp.MVC.Extensions;
using RssSE.WebApp.MVC.Extensions.Interfaces;
using RssSE.WebApp.MVC.Interfaces.Services;
using RssSE.WebApp.MVC.Services;
using RssSE.WebApp.MVC.Services.Handlers;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;

namespace RssSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Attribute Data Annotation
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IIdentityService, IdentityService>(client => 
            {
                client.BaseAddress = new Uri(configuration.GetSection("IdentityBaseServiceUrl").Value);
            })
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));

            services.AddHttpClient<ICatalogService, CatalogService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("CatalogBaseServiceUrl").Value);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.RetryAsyncWithThreeAttemptsAndLogging())
              .AddPolicyHandler(PollyExtensions.CircuitBreakAfterThreeAttempts());

            //services.AddHttpClient("Refit", client =>
            //{
            //    client.BaseAddress = new Uri(configuration.GetSection("CatalogBaseServiceUrl").Value);
            //}).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //   .AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
