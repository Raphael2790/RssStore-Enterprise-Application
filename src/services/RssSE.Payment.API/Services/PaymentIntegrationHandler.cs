using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            SetResponder();
            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<BeganOrderIntegrationEvent, ResponseMessage>
                (async request => await AuthorizePayment(request));
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
