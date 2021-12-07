using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Core.Utils;
using RssSE.MessageBus;
using RssSE.Payment.API.Services;

namespace RssSE.Payment.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PaymentIntegrationHandler>();
        }
    }
}
