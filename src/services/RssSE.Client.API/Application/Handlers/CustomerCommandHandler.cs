using FluentValidation.Results;
using MediatR;
using RssSE.Client.API.Application.Commands;
using RssSE.Client.API.Application.Events;
using RssSE.Client.API.Models;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Core.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Client.API.Application.Handlers
{
    public class CustomerCommandHandler : CommandHandler, 
        IRequestHandler<RegisterCustomerCommand, ValidationResult>,
        IRequestHandler<CreateAddressCommand, ValidationResult>
    {
        private readonly ICustomerRepository _customersRepository;

        public CustomerCommandHandler(ICustomerRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;
            var client = new Customer(message.Id, message.Name, message.Email, message.Cpf);
            if (await _customersRepository.ClientExists(client.Cpf.Number))
            {
                AddError("Este CPF já está em uso");
                return ValidationResult;
            }
            _customersRepository.Add(client);
            client.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));
            return await PersistData(_customersRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(CreateAddressCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;
            var address = new Address(message.Street, message.Number, message.Complement, message.Neighborhood,
                message.ZipCode, message.City, message.State, message.CustomerId);
            _customersRepository.AddAddress(address);
            return await PersistData(_customersRepository.UnitOfWork);
        }
    }
}
