using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssSE.Core.DomainObjects.Exceptions;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Services
{
    public class PaymentIntegrationHandler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBus _bus;

        public PaymentIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            SetResponder();
            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<BeganOrderIntegrationEvent, ResponseMessage>
                (async request => await AuthorizePayment(request));
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<CancelledOrderIntegrationEvent>("PedidoCancelado", async request => await CancelPayment(request));
            _bus.SubscribeAsync<OrderDebitedIntegrationEvent>("PedidoBaixadoEstoque", async request => await CapturePayment(request));
        }

        private async Task CapturePayment(OrderDebitedIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                var response = await paymentService.CapturePayment(message.OrderId);
                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao capturar pagamento ao pedido {message.OrderId}");

                await _bus.PublishAsync(new OrderPayedIntegrationEvent(message.CustomerId, message.OrderId));
            }
        }

        private async Task CancelPayment(CancelledOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                var response = await paymentService.CancelAuthorization(message.OrderId);
                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao cancelar pagamento pedido {message.OrderId}");
            }
        }

        private async Task<ResponseMessage> AuthorizePayment(BeganOrderIntegrationEvent message)
        {
            ResponseMessage response;
            using (var scope = _serviceProvider.CreateScope())
            {
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                var payment = new Models.Payment
                {
                    OrderId = message.OrderId,
                    TotalValue = message.TotalValue,
                    PaymentType = (Models.PaymentType)message.PaymentType,
                    CreditCard = new Models.CreditCard(message.CardOwnerName, message.CardNumber,
                                 message.CardExpirationDate, message.CardCvv)
                };
                response = await paymentService.AuthorizePayment(payment);
            }
            return response;
        }
    }
}
