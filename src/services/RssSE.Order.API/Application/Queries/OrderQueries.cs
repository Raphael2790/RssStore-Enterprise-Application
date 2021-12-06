using Dapper;
using RssSE.Order.API.Application.DTOs;
using RssSE.Order.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Order.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDTO> GetLastOrder(Guid customerId);
        Task<IEnumerable<OrderDTO>> GetListByCustomer(Guid customerId);
    }

    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDTO> GetLastOrder(Guid customerId) 
        {
            const string sql = @"SELECT
                                P.ID AS 'ProductId', 
                                P.CODE, 
                                P.VOUCHERAPPLYED,
                                P.DISCOUNT, 
                                P.TOTALVALUE, 
                                P.ORDERSTATUS,
                                P.STREET, 
                                P.NUMBER, 
                                P.NEIGHBORHOOD, 
                                P.ZIPCODE,
                                P.COMPLEMENT, 
                                P.CITY, 
                                P.STATE, 
                                PIT.ID AS 'ProductItemId',
                                PIT.PRODUCTNAME,
                                PIT.QUANTITY, 
                                PIT.IMAGE, 
                                PIT.UNITVALUE,
                                PIT.IMAGE
                                FROM ORDERS P
                                INNER JOIN ORDERITEMS PIT ON P.ID = PIT.ORDERID
                                WHERE P.CUSTOMERID = @customerId
                                AND P.REGISTERDATE BETWEEN DATEADD(minute, -3, GETDATE()) AND DATEADD(minute, 0, GETDATE())
                                AND P.ORDERSTATUS = 1
                                ORDER BY P.REGISTERDATE DESC";

            var order = await _orderRepository.GetConnection()
                .QueryAsync<dynamic>(sql,new { customerId});

            return MapOrder(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetListByCustomer(Guid customerId)
        {
            var orders = await _orderRepository.GetByCustomerId(customerId);
            return orders.Select(OrderDTO.ToOrderDTO);
        }

        private OrderDTO MapOrder(dynamic result)
        {
            var order = new OrderDTO
            {
                Code = result[0].CODE,
                Status = result[0].ORDERSTATUS,
                TotalValue = result[0].TOTALVALUE,
                Discount = result[0].DISCOUNT,
                VoucherApplyed = result[0].VOUCHERAPPLYED,

                OrderItems = new List<OrderItemDTO>(),
                Address = new AddressDTO
                {
                    Street = result[0].STREET,
                    Neighborhood = result[0].NEIGHBORHOOD,
                    Number = result[0].NUMBER,
                    City = result[0].CITY,
                    State = result[0].STATE,
                    Complement = result[0].COMPLEMENT,
                    ZipCode = result[0].ZIPCODE
                }
            };

            foreach (var item in result)
            {
                var orderItem = new OrderItemDTO
                {
                    Image = item.IMAGE,
                    ProductName = item.PRODUCTNAME,
                    Quantity = item.QUANTITY,
                    UnitValue = item.UNITVALUE
                };
                order.OrderItems.Add(orderItem);
            }

            return order;
        }
    }
}
