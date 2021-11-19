using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssSE.Client.API.Application.Commands;
using RssSE.Core.Mediator;
using RssSE.Core.Messages.Integration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Client.API.Services
{
    public class RegisterCustomerIntegrationHandler : BackgroundService
    {
        private IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisterCustomerIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672");
            _bus.Rpc.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => 
            new ResponseMessage(await RegisterCustomer(request)));
            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegisterCustomer(RegisteredUserIntegrationEvent message)
        {
            var customerCommand = new RegisterCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);
            ValidationResult result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                result = await mediator.SendCommand(customerCommand);
            }
            return result;
        }
    }
}
