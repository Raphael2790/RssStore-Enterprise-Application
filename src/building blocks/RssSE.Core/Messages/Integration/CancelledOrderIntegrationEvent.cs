using System;

namespace RssSE.Core.Messages.Integration
{
    public class CancelledOrderIntegrationEvent : IntegrationEvent
    {
        public CancelledOrderIntegrationEvent(Guid customerId, Guid orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }

        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

    }
}
