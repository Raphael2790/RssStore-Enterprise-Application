using System;
using System.Collections.Generic;

namespace RssSE.Core.Messages.Integration
{
    public class AuthorizedOrderIntegrationEvent : IntegrationEvent
    {
        public AuthorizedOrderIntegrationEvent(Guid customerId, Guid orderId, IDictionary<Guid, int> items)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Items = items;
        }

        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public IDictionary<Guid, int> Items { get; private set; }

    }
}
