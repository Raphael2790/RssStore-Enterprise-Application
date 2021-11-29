using RssSE.Bff.Purchases.Models;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.Interfaces
{
    public interface IOrderService
    {
        Task<VoucherDTO> GetVoucherByCode(string code);
    }
}
