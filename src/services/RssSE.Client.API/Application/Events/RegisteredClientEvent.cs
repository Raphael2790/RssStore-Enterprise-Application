using FluentValidation;
using RssSE.Core.DomainObjects.ValueObjects;
using RssSE.Core.Messages;
using System;

namespace RssSE.Client.API.Application.Events
{
    public class RegisteredClientEvent : Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegisteredClientEvent(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        //Caso o evento possuisse informações necessárias além do comando ou diferentes
        public override bool IsValid()
        {
            ValidationResult = new RegisteredClientEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RegisteredClientEventValidation : AbstractValidator<RegisteredClientEvent>
    {
        public string IdErrorMessage => "Id cliente é inválido";
        public string NameErrorMessage => "Nome do cliente deve ser informado";
        public string CpfErrorMessage => "O CPF do cleinte deve ser válido";
        public string EmailErrorMessage => "O email do cleinte está em formato inválido";
        public RegisteredClientEventValidation()
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
