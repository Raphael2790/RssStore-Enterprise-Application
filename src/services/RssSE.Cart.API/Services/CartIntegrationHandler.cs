using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssSE.Cart.API.Data;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Cart.API.Services
{
    public class CartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CartIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<FinishedOrderIntegrationEvent>("PedidoRealizado", 
                async request => await DeleteCart(request));
        }

        private async Task DeleteCart(FinishedOrderIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CartDbContext>();
            var cart = await context.ClientCarts.FirstOrDefaultAsync(x => x.ClientId == message.CustomerId);
            if(cart != null)
            {
                context.ClientCarts.Remove(cart);
                await context.SaveChangesAsync();
            }
        }
    }
}
