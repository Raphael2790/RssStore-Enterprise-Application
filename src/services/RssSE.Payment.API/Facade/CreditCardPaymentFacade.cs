using Microsoft.Extensions.Options;
using RssSE.Payment.RssSEPag;
using System;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Facade
{
    public class CreditCardPaymentFacade : IPaymentFacade
    {
        private readonly PaymentConfig _paymentConfig;

        public CreditCardPaymentFacade(IOptions<PaymentConfig> paymentConfig)
        {
            _paymentConfig = paymentConfig.Value;
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

        public async Task<Models.Transaction> CancelAuthorization(Models.Transaction transaction)
        {
            var rssPagService = new RssSEPagService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncriptKey);
            var paymentTransaction = ToPaymentTransaction(transaction, rssPagService);
            return ToDomainTransaction(await paymentTransaction.CancelAuthorization());
        }

        public async Task<Models.Transaction> CapturePayment(Models.Transaction transaction)
        {
            var rssPagService = new RssSEPagService(_paymentConfig.DefaultApiKey, _paymentConfig.DefaultEncriptKey);
            var paymentTransaction = ToPaymentTransaction(transaction, rssPagService);
            return ToDomainTransaction(await paymentTransaction.CaptureCardTransaction());
        }

        private Transaction ToPaymentTransaction(Models.Transaction transaction, RssSEPagService rssPagService)
        {
            return new Transaction(rssPagService)
            {
                Status = (TransactionStatus)transaction.TransactionStatus,
                Amount = transaction.TotalValue,
                CardBrand = transaction.CardFlag,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.TransactionCost,
                Nsu = transaction.NSU,
                Tid = transaction.TID
            };
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
