using RssSE.Core.Messages;
using System;

namespace RssSE.Order.API.Application.Events
{
    public class FinishedOrderEvent : Event
    {
        public FinishedOrderEvent(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
    }
}
