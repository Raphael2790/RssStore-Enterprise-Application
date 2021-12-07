using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssSE.Catalog.API.Models;
using RssSE.Catalog.API.Models.Repositories;
using RssSE.Core.DomainObjects.Exceptions;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Services
{
    public class CatalogIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBus _bus;

        public CatalogIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
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
            _bus.SubscribeAsync<AuthorizedOrderIntegrationEvent>("PedidoAutorizado", async request => await DebitStock(request));
        }

        private async Task DebitStock(AuthorizedOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var productsWithStockAvailable = new List<Product>();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var productsIds = string.Join(',', message.Items.Select(x => x.Key));
                var products = await productRepository.GetProductsById(productsIds);

                if(products.Count() != message.Items.Count)
                {
                    CancelOrderWithoutStock(message);
                    return;
                }

                foreach (var product in products)
                {
                    var productQuantity = message.Items.FirstOrDefault(p => p.Key == product.Id).Value;

                    if (product.IsAvailable(productQuantity))
                    {
                        product.DebitStock(productQuantity);
                        productsWithStockAvailable.Add(product);
                    }
                }

                if(productsWithStockAvailable.Count != message.Items.Count)
                {
                    CancelOrderWithoutStock(message);
                }

                foreach (var product in productsWithStockAvailable)
                {
                    productRepository.Update(product);
                }

                //Lancar uma execption fará que a mensagem volte para a fila como mensagem de erro
                //Será processada posteriormente porém pode afetar o fluxo da ordem
                if (!await productRepository.UnitOfWork.Commit())
                    throw new DomainException($"Problemas ao atualizar estoque do pedido {message.OrderId}");

                var orderDebited = new OrderDebitedIntegrationEvent(message.CustomerId, message.OrderId);
                await _bus.PublishAsync(orderDebited);
            }
        }

        private async void CancelOrderWithoutStock(AuthorizedOrderIntegrationEvent message)
        {
            var orderCancelled = new CancelledOrderIntegrationEvent(message.CustomerId, message.OrderId);
            await _bus.PublishAsync(orderCancelled);
        }
    }
}
