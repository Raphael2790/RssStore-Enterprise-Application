using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using RssSE.Order.API.Application.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Order.API.Services
{
    public class OrchestrateOrderIntegrationHandler : IHostedService, IDisposable
    {
        private readonly ILogger<OrchestrateOrderIntegrationHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        public OrchestrateOrderIntegrationHandler(ILogger<OrchestrateOrderIntegrationHandler> logger, 
                                                    IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos iniciado");
            _timer = new Timer(ProcessOrder, null ,TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        private async void ProcessOrder(object state)
        {
            _logger.LogInformation("Processando algum pedido");
            using var scope = _serviceProvider.CreateScope();
            var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQueries>();
            var order = await orderQueries.GetAuthorizedOrders();

            if (order is null) return;

            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
            var authorizedOrder = new AuthorizedOrderIntegrationEvent(order.CustomerId, order.Id,
                order.OrderItems.ToDictionary(p => p.ProductId, p => p.Quantity));
            await bus.PublishAsync(authorizedOrder);
            _logger.LogInformation($"Pedido ID: {order.Id} foi encaminhado para baixa de estoque");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos finalizado");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
