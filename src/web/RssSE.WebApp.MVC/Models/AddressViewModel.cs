using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RssSE.WebApp.MVC.Models
{
    public class AddressViewModel
    {
        public AddressViewModel(string street, string number, string complement, string neighborhood, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public AddressViewModel() {}

        [Required]
        [DisplayName("Logradouro")]
        public string Street { get; set; }
        [Required]
        [DisplayName("Número")]
        public string Number { get; set; }
        [Required]
        [DisplayName("Complemento")]
        public string Complement { get; set; }
        [Required]
        [DisplayName("Bairro")]
        public string Neighborhood { get; set; }
        [Required]
        [DisplayName("CEP")]
        public string ZipCode { get; set; }
        [Required]
        [DisplayName("Cidade")]
        public string City { get; set; }
        [Required]
        [DisplayName("Estado")]
        public string State { get; set; }

        public override string ToString()
        {
            return $"{Street}, {Number} {Complement} - {Neighborhood} - {City} - {State}";
        }
    }
}
