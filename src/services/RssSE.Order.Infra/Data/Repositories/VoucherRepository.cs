using Microsoft.EntityFrameworkCore;
using RssSE.Core.Data;
using RssSE.Order.Domain.Entities;
using RssSE.Order.Domain.Repositories;
using RssSE.Order.Infra.Data.Context;
using System.Threading.Tasks;

namespace RssSE.Order.Infra.Data.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly OrdersDbContext _context;
        public VoucherRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose() => _context?.Dispose();

        public async Task<Voucher> GetVoucherByCode(string code) =>
            await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code);

        public void Update(Voucher voucher) => _context.Vouchers.Update(voucher);
    }
}
