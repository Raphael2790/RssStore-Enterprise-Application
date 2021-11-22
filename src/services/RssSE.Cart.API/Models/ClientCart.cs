using System;
using System.Collections.Generic;
using System.Linq;

namespace RssSE.Cart.API.Models
{
    public class ClientCart
    {
        internal const int MAX_QUANTITY_ITEM = 5;

        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal TotalValue { get; set; }
        public List<CartItem> CartItems { get; set; }

        public ClientCart(Guid clientId)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
        }

        protected ClientCart() { }

        internal void CalculateCartTotal() => TotalValue = CartItems.Sum(c => c.CalculateItemValue());

        internal bool ItemExistsInCart(CartItem item) => CartItems.Any(x => x.ProductId == item.ProductId);

        internal CartItem GetItemByProductId(Guid productId) => CartItems.FirstOrDefault(x => x.ProductId == productId);

        internal void AddItem(CartItem item)
        {
            if (!item.IsValid()) return;
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
    }
}
