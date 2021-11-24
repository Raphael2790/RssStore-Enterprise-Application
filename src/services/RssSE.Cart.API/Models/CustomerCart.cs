using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RssSE.Cart.API.Models
{
    public class CustomerCart
    {
        internal const int MAX_QUANTITY_ITEM = 5;

        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal TotalValue { get; set; }
        public List<CartItem> CartItems { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public CustomerCart(Guid clientId)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
            CartItems = new List<CartItem>();
        }

        protected CustomerCart() { }

        internal void CalculateCartTotal() => TotalValue = CartItems.Sum(c => c.CalculateItemValue());

        internal bool ItemExistsInCart(CartItem item) => CartItems.Any(x => x.ProductId == item.ProductId);

        internal CartItem GetItemByProductId(Guid productId) => CartItems.FirstOrDefault(x => x.ProductId == productId);

        internal void AddItem(CartItem item)
        {
            item.AssociateCart(Id);
            if (ItemExistsInCart(item))
            {
                var existingItem = GetItemByProductId(item.ProductId);
                existingItem.AddQuantity(item.Quantity);
                item = existingItem;
                CartItems.Remove(existingItem);
            }
            CartItems.Add(item);
            CalculateCartTotal();
        }
        
        internal void UpdateItem(CartItem item)
        {
            item.AssociateCart(Id);
            var existingItem = GetItemByProductId(item.ProductId);
            CartItems.Remove(existingItem);
            CartItems.Add(item);
            CalculateCartTotal();
        }

        internal void UpdateQuantity(CartItem item, int quantity)
        {
            item.UpdateQuantity(quantity);
            UpdateItem(item);
        }

        internal void RemoveItem(CartItem item)
        {
            CartItems.Remove(GetItemByProductId(item.ProductId));
            CalculateCartTotal();
        }

        internal bool IsValid()
        {
            var erros = CartItems.SelectMany(i => new CartItemValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CustomerCartValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(erros);
            return ValidationResult.IsValid;
        }
    }

    public class CustomerCartValidation : AbstractValidator<CustomerCart>
    {
        public readonly string CustomerIdErrorMsg = "Não foi informado a identificação do cliente";
        public readonly string CartItensErrorMsg = "O carrinho não possui itens";
        public readonly string TotalCartValueErrorMsg = "O valor total do carrinho precisa ser maior que 0";
        public CustomerCartValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMsg)
                .NotEmpty()
                .WithMessage(CustomerIdErrorMsg);

            RuleFor(c => c.CartItems.Count)
                .GreaterThan(default(int))
                .WithMessage(CartItensErrorMsg);

            RuleFor(c => c.TotalValue)
                .GreaterThan(decimal.Zero)
                .WithMessage(TotalCartValueErrorMsg);
        }
    }
}
