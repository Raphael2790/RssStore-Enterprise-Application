using FluentValidation;
using RssSE.Core.Messages;
using RssSE.Order.API.Application.DTOs;
using System;
using System.Collections.Generic;

namespace RssSE.Order.API.Application.Commands
{
    public class CreateOrderCommand : Command
    {
        public Guid CustomerId { get; set; }
        public decimal TotalValue { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }

        public string VoucherCode { get; set; }
        public bool VoucherApplyed { get; set; }
        public decimal Discount { get; set; }

        public AddressDTO Address { get; set; }

        public string CardNumber { get; set; }
        public string CardOwnerName { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateOrderCommandValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido")
                .NotNull()
                .WithMessage("Id do cliente inválido");

            RuleFor(x => x.OrderItems.Count)
                .GreaterThan(0)
                .WithMessage("O pedido precisa ter no mínimo 1 item");

            RuleFor(x => x.CardNumber)
                .CreditCard()
                .WithMessage("Número do cartão inválido");

            RuleFor(x => x.CardOwnerName)
                .NotNull()
                .WithMessage("Nome do portador do cartão deve ser informado");

            RuleFor(x => x.CardCvv.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("O CVV do cartão precisa ter entre 2 e 4 números");

            RuleFor(x => x.CardExpirationDate)
                .NotNull()
                .WithMessage("A data de validade do cartão deve ser informada");
        }
    }
}
