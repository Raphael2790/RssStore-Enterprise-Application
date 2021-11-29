using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Cart.API.Models
{
    public class Voucher
    {
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public string Code { get; set; }
        public VoucherType VoucherType { get; set; }
    }

    public enum VoucherType
    {
        Percentage = 0,
        Value = 1
    }
}
