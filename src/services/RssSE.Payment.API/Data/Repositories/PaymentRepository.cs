using RssSE.Core.Data;
using RssSE.Payment.API.Data.Context;
using RssSE.Payment.API.Models.Interfaces;

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

        public void AddPayment(Models.Payment payment) => _context.Payments.Add(payment);

        public void Dispose() => _context?.Dispose();
    }
}
