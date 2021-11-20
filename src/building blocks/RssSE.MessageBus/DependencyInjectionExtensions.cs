using Microsoft.Extensions.DependencyInjection;
using System;

namespace RssSE.MessageBus
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            //devido o construtor necessitar de parametros que não são providos via injeção de dependencia
            services.AddSingleton<IMessageBus>(new MessageBus(connectionString));
            return services;
        }
    }
}
