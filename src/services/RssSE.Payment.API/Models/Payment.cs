using RssSE.Core.DomainObjects.BaseEntity;
using RssSE.Core.DomainObjects.Interfaces;
using System;
using System.Collections.Generic;

namespace RssSE.Payment.API.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Guid OrderId { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal TotalValue { get; set; }

        public CreditCard CreditCard { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public Payment()
        {
            Transactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction transaction) => Transactions.Add(transaction);
    }
}
