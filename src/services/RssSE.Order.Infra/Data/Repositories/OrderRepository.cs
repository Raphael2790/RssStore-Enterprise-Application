using Microsoft.EntityFrameworkCore;
using RssSE.Core.Data;
using RssSE.Order.Domain.Entities;
using RssSE.Order.Domain.Repositories;
using RssSE.Order.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Order.Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository, IRepository<Domain.Entities.Order>
    {
        private readonly OrdersDbContext _context;

        public OrderRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public void AddOrder(Domain.Entities.Order order) => _context.Orders.Add(order);

        public void Dispose() => _context?.Dispose();

        public async Task<Domain.Entities.Order> Get(Guid id) => await _context.Orders.FindAsync(id);

        public async Task<IEnumerable<Domain.Entities.Order>> GetByCustomerId(Guid customerId) =>
            await _context.Orders
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();

        public async Task<OrderItem> GetItemById(Guid id) => await _context.OrdersItems.FindAsync(id);

        public async Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId) =>
            await _context.OrdersItems.FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId);

        public void UpdateOrder(Domain.Entities.Order order) => _context.Orders.Update(order);
    }
}
