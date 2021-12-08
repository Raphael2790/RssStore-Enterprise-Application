using FluentValidation.Results;
using RssSE.Core.DomainObjects.Exceptions;
using RssSE.Core.Messages.Integration;
using RssSE.Payment.API.Facade;
using RssSE.Payment.API.Models;
using RssSE.Payment.API.Models.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFacade _paymentFacade;
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentFacade paymentFacade, 
                              IPaymentRepository paymentRepository)
        {
            _paymentFacade = paymentFacade;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseMessage> AuthorizePayment(Models.Payment payment)
        {
            var transaction = await _paymentFacade.AuthorizePayment(payment);
            var validationResult = new ValidationResult();

            if(transaction.TransactionStatus != Models.TransactionStatus.Authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento recusado, entre em contato " +
                    "com a operadora do seu cartão"));
                return new ResponseMessage(validationResult);
            }

            payment.AddTransaction(transaction);
            _paymentRepository.AddPayment(payment);

            if(!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar pagamento"));
                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public Task<ResponseMessage> CancelAuthorization(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseMessage> CapturePayment(Guid orderId)
        {
            var transactions = await _paymentRepository.GetTransactionsByOrderId(orderId);
            var authorizedTransaction = transactions.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction is null) throw new DomainException($"Transação não encontrada para o pedido {orderId}");

            var transaction = await _paymentFacade.CapturePayment(authorizedTransaction);

            if(!(transaction.TransactionStatus is TransactionStatus.Payed))
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", $"Não foi possível capturar o pagamento do pedido {orderId}"));
                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = authorizedTransaction.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if(!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", $"Não foi possível persistir a captura do pedido {orderId}"));
                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
