using RssSE.Core.DomainObjects.BaseEntity;
using RssSE.Core.DomainObjects.Interfaces;
using RssSE.Order.Domain.Enums;
using RssSE.Order.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RssSE.Order.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherApplyed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public Address Address { get; private set; }
        public Voucher Voucher { get; private set; }

        public Order(Guid customerId, decimal totalValue, List<OrderItem> items, 
            bool voucherApplyed = false, decimal discount = 0, Guid? voucherId = null)
        {
            CustomerId = customerId;
            TotalValue = totalValue;
            _orderItems = items;
            VoucherApplyed = voucherApplyed;
            Discount = discount;
            VoucherId = voucherId;
        }

        protected Order() { }

        public void AuthorizeOrder() => OrderStatus = OrderStatus.Authorized;

        public void AddVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherApplyed = true;
            VoucherId = voucher.Id;
        }

        public void AddAddress(Address address) => Address = address;

        public void CalculateOrderValue()
        {
            TotalValue = OrderItems.Sum(i => i.CalculateValue());
            CalculateTotalDiscountValue();
        }

        public void CalculateTotalDiscountValue()
        {
            if (!VoucherApplyed) return;
            decimal discount = 0;
            var totalValue = TotalValue;
            if(Voucher.VoucherType == VoucherType.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (totalValue * Voucher.Percentage.Value) / 100;
                    totalValue -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    totalValue -= discount;
                }
            }

            Discount = discount;
            TotalValue = totalValue < 0 ? 0 : totalValue; 
        }
    }
}
