using System;
using System.Collections.Generic;

namespace RssSE.WebApp.MVC.Models
{
    public class OrderViewModel
    {
        public int Code { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public bool VoucherApplyed { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
        public AddressViewModel Address { get; set; }
    }

    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }
        public string Image { get; set; }
    }
}
