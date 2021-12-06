using System;

namespace RssSE.Core.Messages.Integration
{
    public class BeganOrderIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public int PaymentType { get; set; }
        public decimal TotalValue { get; set; }

        public string CardNumber { get; set; }
        public string CardOwnerName { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }
    }
}
