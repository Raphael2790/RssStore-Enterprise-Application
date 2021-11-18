using FluentValidation.Results;
using MediatR;
using RssSE.Core.Messages;
using System.Threading.Tasks;

namespace RssSE.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T @event) where T : Event => await _mediator.Publish(@event);

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command => await _mediator.Send(command);
    }
}
