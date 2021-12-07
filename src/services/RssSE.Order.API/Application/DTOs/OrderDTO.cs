using System;
using System.Collections.Generic;

namespace RssSE.Order.API.Application.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public Guid CustomerId { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherApplyed { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; }
        public AddressDTO Address { get; set; }

        public static OrderDTO ToOrderDTO(Domain.Entities.Order order)
        {
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                Date = order.RegisterDate,
                TotalValue = order.TotalValue,
                Discount = order.Discount,
                VoucherApplyed = order.VoucherApplyed,
                Status = (int)order.OrderStatus,
                VoucherCode = order.VoucherApplyed ? order.Voucher?.Code : null,
                OrderItems = new List<OrderItemDTO>()
            };

            foreach (var item in order.OrderItems)
            {
                orderDTO.OrderItems.Add(new OrderItemDTO
                {
                    ProductName = item.ProductName,
                    Image = item.Image,
                    Quantity = item.Quantity,
                    UnitValue = item.UnitValue,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId
                });
            }

            orderDTO.Address = new AddressDTO
            {
                Street = order.Address.Street,
                Number = order.Address.Number,
                Complement = order.Address.Complement,
                City = order.Address.City,
                Neighborhood = order.Address.Neighborhood,
                State = order.Address.State,
                ZipCode = order.Address.ZipCode
            };

            return orderDTO;
        }
    }
}
