using FluentValidation;
using RssSE.Core.DomainObjects.ValueObjects;
using RssSE.Core.Messages;
using System;

namespace RssSE.Client.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegisterCustomerCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid() 
        {
            ValidationResult = new RegisterClientCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RegisterClientCommandValidation : AbstractValidator<RegisterCustomerCommand>
    {
        public string IdErrorMessage => "Id cliente é inválido";
        public string NameErrorMessage => "Nome do cliente deve ser informado";
        public string CpfErrorMessage => "O CPF do cleinte deve ser válido";
        public string EmailErrorMessage => "O email do cleinte está em formato inválido";
        public RegisterClientCommandValidation()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage(IdErrorMessage)
                .NotNull()
                .WithMessage(IdErrorMessage);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(NameErrorMessage)
                .NotNull()
                .WithMessage(NameErrorMessage);

            RuleFor(x => x.Cpf)
                .Must(HasValidCpf)
                .WithMessage(CpfErrorMessage);

            RuleFor(x => x.Email)
                .Must(HasValidEmail)
                .WithMessage(EmailErrorMessage);
        }

        protected static bool HasValidCpf(string cpf) => Cpf.Validate(cpf);
        protected static bool HasValidEmail(string email) => Email.Validate(email);
    }
}
