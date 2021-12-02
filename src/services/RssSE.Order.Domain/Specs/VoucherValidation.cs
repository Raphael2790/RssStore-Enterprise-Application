using NetDevPack.Specification;
using RssSE.Order.Domain.Entities;

namespace RssSE.Order.Domain.Specs
{
    public class VoucherValidation : SpecValidator<Voucher>
    {
        public VoucherValidation()
        {
            var dateSpec = new VoucherDateSpecification();
            var quantSpec = new VoucherAmountSpecification();
            var activeSpec = new VoucherActiveSpecification();

            Add("dateSpec", new Rule<Voucher>(dateSpec, "Este voucher está expirado"));
            Add("quantSpec", new Rule<Voucher>(quantSpec, "Este voucher já foi utilizado"));
            Add("activeSpec", new Rule<Voucher>(activeSpec, "Este voucher não está mais ativo"));
        }
    }
}
