using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Services.gRPC;
using System;

namespace RssSE.Bff.Purchases.Configuration
{
    public static class GrpcConfig
    {
        public static void AddGrpcServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICartGrpcService, CartGrpcService>();

            services.AddGrpcClient<PurchaseCart.PurchaseCartClient>(options => 
            {
                options.Address = new Uri(configuration["CartAPIUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();
        }
    }
}
