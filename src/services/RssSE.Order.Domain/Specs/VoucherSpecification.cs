using NetDevPack.Specification;
using RssSE.Order.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace RssSE.Order.Domain.Specs
{
    public class VoucherDateSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.ExpirationDate > DateTime.Now;
        }
    }

    public class VoucherAmountSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Amount > 0;
        }
    }

    public class VoucherActiveSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Active && !voucher.Applyed;
        }
    }
}
