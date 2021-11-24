using FluentValidation;
using System;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public CustomerCart ClientCart { get; set; }

        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        internal void AssociateCart(Guid cartId) =>
            CartId = cartId;

        internal decimal CalculateItemValue() => Quantity * UnitValue;

        internal void AddQuantity(int quantity) => Quantity += quantity;

        internal bool IsValid() => new CartItemValidation().Validate(this).IsValid;

        internal void UpdateQuantity(int quantity) => Quantity = quantity;
    }

    public class CartItemValidation : AbstractValidator<CartItem>
    {
        public string ProductIdErrorMsg(string itemName) => $"O id do produto do item {itemName} está inválido";
        public readonly string ProductNameErrorMsg = "O nome do produto está inválido";
        public string ItemMinimumQuantityErrorMsg(string itemName) => $"A quantidade mínima do {itemName} é 1";
        public string ItemMaximumQuantityErrorMsg(string itemName) => $"A quantidade máxima do {itemName} é {CustomerCart.MAX_QUANTITY_ITEM}";
        public string ItemMinimumValueErrorMsg(string itemName) => $"O valor mínimo do {itemName} deve ser maior que 0";

        public CartItemValidation()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(item => ProductIdErrorMsg(item.Name))
                .NotNull()
                .WithMessage(item => ProductIdErrorMsg(item.Name));

            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage(ProductNameErrorMsg)
                .NotEmpty()
                .WithMessage(ProductNameErrorMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(default(int))
                .WithMessage(item => ItemMinimumQuantityErrorMsg(item.Name));

            RuleFor(c => c.Quantity)
                .LessThanOrEqualTo(CustomerCart.MAX_QUANTITY_ITEM)
                .WithMessage(item => ItemMaximumQuantityErrorMsg(item.Name));

            RuleFor(c => c.UnitValue)
                .GreaterThan(decimal.Zero)
                .WithMessage(item => ItemMinimumValueErrorMsg(item.Name));
        }
    }
}
