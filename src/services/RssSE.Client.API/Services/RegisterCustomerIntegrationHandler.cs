using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssSE.Client.API.Application.Commands;
using RssSE.Core.Mediator;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Client.API.Services
{
    public class RegisterCustomerIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisterCustomerIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request =>
                await RegisterCustomer(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => 
                await RegisterCustomer(request));
            return Task.CompletedTask;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegisterCustomer(RegisteredUserIntegrationEvent message)
        {
            var customerCommand = new RegisterCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);
            ValidationResult result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                result = await mediator.SendCommand(customerCommand);
            }
            return new ResponseMessage(result);
        }
    }
}
