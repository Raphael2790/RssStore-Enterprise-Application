using RssSE.Core.Data;
using RssSE.Order.Domain.Entities;
using System.Threading.Tasks;

namespace RssSE.Order.Domain.Repositories
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> GetVoucherByCode(string code);
        void Update(Voucher voucher);
    }
}
