using FluentValidation;
using System;

namespace RssSE.Cart.API.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string  Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }
        public string Image { get; set; }
        public Guid CartId { get; set; }

        public ClientCart ClientCart { get; set; }

        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        internal void AssociateCart(Guid cartId) =>
            CartId = cartId;

        internal decimal CalculateItemValue() => Quantity * UnitValue;

        internal void AddQuantity(int quantity) => Quantity += quantity;

        internal bool IsValid() => new CartItemValidation().Validate(this).IsValid;
    }

    public class CartItemValidation : AbstractValidator<CartItem>
    {
        public readonly string ProductIdErrorMsg = "O id do produto está inválido";
        public readonly string ProductNameErrorMsg = "O nome do produto está inválido";
        public readonly string ItemMinimumQuantityErrorMsg = "A quantidade mínima do item é 1";
        public readonly string ItemMaximumQuantityErrorMsg = $"A quantidade máxima do item é {ClientCart.MAX_QUANTITY_ITEM}";
        public readonly string ItemMinimumValueErrorMsg = "O valor mínimo de um item deve ser maior que 0";

        public CartItemValidation()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMsg)
                .NotNull()
                .WithMessage(ProductIdErrorMsg);

            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage(ProductNameErrorMsg)
                .NotEmpty()
                .WithMessage(ProductNameErrorMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(ItemMinimumQuantityErrorMsg);

            RuleFor(c => c.Quantity)
                .LessThan(ClientCart.MAX_QUANTITY_ITEM)
                .WithMessage(ItemMaximumQuantityErrorMsg);

            RuleFor(c => c.UnitValue)
                .GreaterThan(0)
                .WithMessage(ItemMinimumValueErrorMsg);
        }
    }
}
