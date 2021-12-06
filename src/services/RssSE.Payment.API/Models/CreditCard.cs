namespace RssSE.Payment.API.Models
{
    public class CreditCard
    {
        public string CardOwnerName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCVV { get; set; }

        protected CreditCard() { }

        public CreditCard(string cardOwnerName, string cardNumber, string cardExpirationDate, string cardCVV)
        {
            CardOwnerName = cardOwnerName;
            CardNumber = cardNumber;
            CardExpirationDate = cardExpirationDate;
            CardCVV = cardCVV;
        }
    }
}
