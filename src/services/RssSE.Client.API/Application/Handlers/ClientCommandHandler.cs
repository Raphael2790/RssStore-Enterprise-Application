using FluentValidation.Results;
using MediatR;
using RssSE.Client.API.Application.Commands;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Core.Messages;
using System.Threading;
using System.Threading.Tasks;
using ClientModel = RssSE.Client.API.Models.Client;

namespace RssSE.Client.API.Application.Handlers
{
    public class ClientCommandHandler : CommandHandler, IRequestHandler<RegisterClientCommand, ValidationResult>
    {
        private readonly IClientRepository _clientRepository;

        public ClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ValidationResult> Handle(RegisterClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;
            var client = new ClientModel(message.Id, message.Name, message.Email, message.Cpf);
            if (await _clientRepository.ClientExists(client.Cpf.Number))
            {
                AddError("Este CPF já está em uso");
                return ValidationResult;
            }
            _clientRepository.Add(client);
            return await PersistData(_clientRepository.UnitOfWork);
        }
    }
}
