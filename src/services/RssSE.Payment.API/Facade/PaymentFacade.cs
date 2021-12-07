using RssSE.Payment.RssSEPag;
using System;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Facade
{
    public class PaymentFacade : IPaymentFacade
    {
        private readonly PaymentConfig _paymentConfig;

        public PaymentFacade(PaymentConfig paymentConfig)
        {
            _paymentConfig = paymentConfig;
        }

        public async Task<Models.Transaction> AuthorizePayment(Models.Payment payment)
        {
            var rssPagService = new RssSEPagService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncriptKey);

            var cardHashGen = new CardHash(rssPagService)
            {
                CardNumber = payment.CreditCard.CardNumber,
                CardCvv = payment.CreditCard.CardCVV,
                CardExpirationDate = payment.CreditCard.CardExpirationDate,
                CardHolderName = payment.CreditCard.CardOwnerName
            };

            var cardHash = cardHashGen.Generate();

            var transaction = new RssSEPag.Transaction(rssPagService)
            {
                CardHash = cardHash,
                CardNumber = payment.CreditCard.CardNumber,
                CardCvv = payment.CreditCard.CardCVV,
                CardExpirationDate = payment.CreditCard.CardExpirationDate,
                CardHolderName = payment.CreditCard.CardOwnerName,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = payment.TotalValue
            };

            return ToDomainTransaction(await transaction.AuthorizeCardTransaction());
        }

        public Models.Transaction ToDomainTransaction(RssSEPag.Transaction transaction)
        {
            return new Models.Transaction
            {
                Id = Guid.NewGuid(),
                TransactionStatus = (Models.TransactionStatus)transaction.Status,
                CardFlag = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                NSU = transaction.Nsu,
                TID = transaction.Tid,
                TotalValue = transaction.Amount,
                TransactionCost = transaction.Cost,
                TransactionDate = transaction.TransactionDate
            };
        }
    }
}
