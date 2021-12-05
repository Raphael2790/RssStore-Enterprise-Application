using RssSE.Core.Data;
using RssSE.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using CustomerOrder = RssSE.Order.Domain.Entities.Order;

namespace RssSE.Order.Domain.Repositories
{
    public interface IOrderRepository : IRepository<CustomerOrder>
    {
        Task<CustomerOrder> Get(Guid id);
        Task<IEnumerable<CustomerOrder>> GetByCustomerId(Guid customerId);
        void AddOrder(CustomerOrder order);
        void UpdateOrder(CustomerOrder order);

        DbConnection GetConnection();

        Task<OrderItem> GetItemById(Guid id);
        Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId);
    }
}
