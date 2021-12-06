using RssSE.Core.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RssSE.WebApp.MVC.Models
{
    public class OrderTransactionViewModel
    {
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherApplyed { get; set; }

        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public AddressViewModel Address { get; set; }

        [Required(ErrorMessage = "Informe o número do cartão")]
        [DisplayName("Número do Cartão")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string CardOwnerName { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]
        [Required(ErrorMessage = "Informe o vencimento")]
        [ExpirationCard(ErrorMessage = "Cartão Expirado")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string CardExpirationDate { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [DisplayName("Código de Segurança")]
        public string CardCvv { get; set; }
    }
}
