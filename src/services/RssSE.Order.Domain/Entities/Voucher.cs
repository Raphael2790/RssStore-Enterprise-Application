using RssSE.Core.DomainObjects.BaseEntity;
using RssSE.Core.DomainObjects.Interfaces;
using RssSE.Order.Domain.Specs;
using System;

namespace RssSE.Order.Domain.Entities
{
    public class Voucher : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Amount { get; private set; }
        public VoucherType VoucherType { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? ApplyedDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Applyed { get; private set; }

        public bool IsValidForAplly()
        {
            return new VoucherDateSpecification()
                .And(new VoucherActiveSpecification())
                .And(new VoucherAmountSpecification())
                .IsSatisfiedBy(this);
        }

        public void MakeApplyed()
        {
            Applyed = true;
            Active = false;
            Amount = 0;
            ApplyedDate = DateTime.Now;
        }

        public void DebitAmount()
        {
            Amount -= 1;
            if (Amount >= 1) return;
            MakeApplyed();
        }
    }

    public enum VoucherType
    {
        Discount = 1,
        Percentage = 0
    }
}
