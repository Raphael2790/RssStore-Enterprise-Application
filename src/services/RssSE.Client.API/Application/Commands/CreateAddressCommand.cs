using FluentValidation;
using RssSE.Core.Messages;
using System;

namespace RssSE.Client.API.Application.Commands
{
    public class CreateAddressCommand : Command
    {
        public Guid CustomerId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public CreateAddressCommand()
        {
            Country = "Brasil";
        }

        public CreateAddressCommand(Guid customerId, string street, string number, string complement, string neighborhood, string zipCode, string city, string state, string country = "Brasil")
        {
            AggregateId = customerId;
            CustomerId = customerId;
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            Country = country;
        }

        public void SetCustomer(Guid customerId)
        {
            CustomerId = customerId;
        }
        public override bool IsValid()
        {
            ValidationResult = new CreateAddressCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateAddressCommandValidation : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidation()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage("Informe o logradouro")
                .NotNull()
                .WithMessage("Informe o logradouro");

            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage("Informe o número")
                .NotNull()
                .WithMessage("Informe o número");

            RuleFor(x => x.Neighborhood)
                .NotEmpty()
                .WithMessage("Informe o bairro")
                .NotNull()
                .WithMessage("Informe o bairro");

            RuleFor(x => x.ZipCode)
                .NotNull()
                .WithMessage("Informe o CEP")
                .NotEmpty()
                .WithMessage("Informe o CEP");

            RuleFor(x => x.City)
                .NotNull()
                .WithMessage("Informe a cidade")
                .NotEmpty()
                .WithMessage("Informe a cidade");

            RuleFor(x => x.State)
                .NotNull()
                .WithMessage("Informe o estado")
                .NotEmpty()
                .WithMessage("Informe o estado");

            RuleFor(x => x.Country)
                .NotNull()
                .WithMessage("Informe o pais")
                .NotEmpty()
                .WithMessage("Informe o pais");
        }
    }
}
