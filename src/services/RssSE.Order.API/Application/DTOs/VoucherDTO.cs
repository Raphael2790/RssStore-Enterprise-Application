namespace RssSE.Order.API.Application.DTOs
{
    public class VoucherDTO
    {
        public string Code { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public int TipoDesconto { get; set; }
    }
}
