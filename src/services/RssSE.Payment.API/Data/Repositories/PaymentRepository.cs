using Microsoft.EntityFrameworkCore;
using RssSE.Core.Data;
using RssSE.Payment.API.Data.Context;
using RssSE.Payment.API.Models;
using RssSE.Payment.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;
        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId) =>
            await _context.Transactions.AsNoTracking().Where(t => t.Payment.OrderId == orderId).ToListAsync();

        public void AddPayment(Models.Payment payment) => _context.Payments.Add(payment);
        public void AddTransaction(Transaction transaction) => _context.Transactions.Add(transaction);

        public void Dispose() => _context?.Dispose();
    }
}
