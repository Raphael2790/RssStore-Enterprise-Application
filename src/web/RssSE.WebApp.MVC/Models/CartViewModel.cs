using System;
using System.Collections.Generic;

namespace RssSE.WebApp.MVC.Models
{
    public class CartViewModel
    {
        public decimal TotalValue { get; set; }
        public List<ProductItemViewModel> CartItems { get; set; } = new List<ProductItemViewModel>();
    }

    public class ProductItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }
        public string Image { get; set; }
    }
}
