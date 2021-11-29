using System;
using System.Collections.Generic;

namespace RssSE.WebApp.MVC.Models
{
    public class CartViewModel
    {
        public decimal TotalValue { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public bool VoucherApplyed { get; set; }
        public decimal Discount { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    }

    public class CartItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }
        public string Image { get; set; }
    }
}
