using RssSE.Core.Messages.Integration;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Services
{
    public interface IPaymentService
    {
        Task<ResponseMessage> AuthorizePayment(Models.Payment payment);
    }
}
