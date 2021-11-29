using RssSE.Order.API.Application.DTOs;
using RssSE.Order.Domain.Repositories;
using System.Threading.Tasks;

namespace RssSE.Order.API.Application.Queries
{
    public interface IVoucherQuery
    {
        Task<VoucherDTO> GetVoucherByCode(string code);
    }

    public class VoucherQuery : IVoucherQuery
    {
        private readonly IVoucherRepository _voucherRepository;
        public VoucherQuery(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherByCode(code);

            if (voucher is null) return null;

            if (!voucher.IsValidForAplly()) return null;

            return new VoucherDTO
            {
                Code = voucher.Code,
                DiscountValue = voucher.DiscountValue,
                Percentage = voucher.Percentage,
                TipoDesconto = (int)voucher.VoucherType
            };
        }
    }
}
