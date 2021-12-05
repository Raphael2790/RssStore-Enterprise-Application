using System;

namespace RssSE.Core.Messages.Integration
{
    public class FinishedOrderIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }

        public FinishedOrderIntegrationEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
