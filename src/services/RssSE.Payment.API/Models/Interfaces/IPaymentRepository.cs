using RssSE.Core.Data;

namespace RssSE.Payment.API.Models.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void AddPayment(Payment payment);
    }
}
