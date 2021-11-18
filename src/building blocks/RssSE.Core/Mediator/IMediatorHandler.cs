using FluentValidation.Results;
using RssSE.Core.Messages;
using System.Threading.Tasks;

namespace RssSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}
