using RssSE.Order.Domain.Entities;
using System;

namespace RssSE.Order.API.Application.DTOs
{
    public class OrderItemDTO
    {
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public decimal Value { get; set; }
        public Guid OrderId { get; set; }

        public static OrderItem ToOrderItem(OrderItemDTO orderItem) =>
            new OrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.Quantity,
                orderItem.Value, orderItem.Image);
    }
}
