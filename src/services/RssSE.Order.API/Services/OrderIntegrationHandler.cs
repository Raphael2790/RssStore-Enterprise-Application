using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssSE.Core.DomainObjects.Exceptions;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using RssSE.Order.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Order.API.Services
{
    public class OrderIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBus _bus;
        public OrderIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderPayedIntegrationEvent>("PedidoPago", async message => await FinishOrder(message));
            _bus.SubscribeAsync<CancelledOrderIntegrationEvent>("PedidoCancelado", async message => await CancelOrder(message));
        }

        private async Task CancelOrder(CancelledOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var order = await orderRepository.Get(message.OrderId);
                order.CancelOrder();
                orderRepository.UpdateOrder(order);
                if (!await orderRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"Problemas ao finalizar o pedido {message.OrderId}");
                }
            }
        }

        private async Task FinishOrder(OrderPayedIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var order = await orderRepository.Get(message.OrderId);
                order.FinishOrder();
                orderRepository.UpdateOrder(order);
                if(!await orderRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"Problemas ao finalizar o pedido {message.OrderId}");
                }
            }
        }
    }
}
