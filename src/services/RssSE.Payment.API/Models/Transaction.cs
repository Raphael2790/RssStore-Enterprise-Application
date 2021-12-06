using RssSE.Core.DomainObjects.BaseEntity;
using System;

namespace RssSE.Payment.API.Models
{
    public class Transaction : Entity
    {
        public string AuthorizationCode { get; set; }
        public string CardFlag { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TransactionCost { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TID { get; set; }
        public string NSU { get; set; }

        public Guid PaymentId { get; set; }

        public Payment Payment { get; set; }
    }
}
