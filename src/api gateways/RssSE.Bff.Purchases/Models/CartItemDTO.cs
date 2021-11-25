using System;

namespace RssSE.Bff.Purchases.Models
{
    public class CartItemDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }
        public string Image { get; set; }
    }
}
