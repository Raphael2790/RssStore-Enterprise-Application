using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Models.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void AddPayment(Payment payment);
        void AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId);
    }
}
