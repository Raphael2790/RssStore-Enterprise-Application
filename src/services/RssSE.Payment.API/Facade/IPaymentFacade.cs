using RssSE.Payment.API.Models;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Facade
{
    public interface IPaymentFacade
    {
        Task<Transaction> AuthorizePayment(Models.Payment payment);
    }
}
