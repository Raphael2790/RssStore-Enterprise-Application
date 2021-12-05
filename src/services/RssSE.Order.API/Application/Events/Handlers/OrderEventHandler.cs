using MediatR;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Order.API.Application.Events.Handlers
{
    public class OrderEventHandler : INotificationHandler<FinishedOrderEvent>
    {
        private readonly IMessageBus _bus;

        public OrderEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(FinishedOrderEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new FinishedOrderIntegrationEvent(message.CustomerId));
        }
    }
}
