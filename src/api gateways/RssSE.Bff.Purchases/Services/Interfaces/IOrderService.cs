using RssSE.Bff.Purchases.Models;
using RssSE.Core.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseResult> FinishOrder(OrderDTO order);
        Task<OrderDTO> GetLastOrder();
        Task<IEnumerable<OrderDTO>> GetListByCustomerId();
        Task<VoucherDTO> GetVoucherByCode(string code);
    }
}
