using RssSE.Core.Messages.Integration;
using System;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Services
{
    public interface IPaymentService
    {
        Task<ResponseMessage> AuthorizePayment(Models.Payment payment);
        Task<ResponseMessage> CapturePayment(Guid orderId);
        Task<ResponseMessage> CancelAuthorization(Guid orderId);
    }
}
