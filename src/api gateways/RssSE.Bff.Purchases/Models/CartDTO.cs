using System.Collections.Generic;

namespace RssSE.Bff.Purchases.Models
{
    public class CartDTO
    {
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }
}
