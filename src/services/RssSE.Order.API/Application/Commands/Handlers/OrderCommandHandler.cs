﻿using FluentValidation.Results;
using MediatR;
using RssSE.Core.Mediator;
using RssSE.Core.Messages;
using RssSE.Core.Messages.Integration;
using RssSE.MessageBus;
using RssSE.Order.API.Application.DTOs;
using RssSE.Order.API.Application.Events;
using RssSE.Order.Domain.Repositories;
using RssSE.Order.Domain.Specs;
using RssSE.Order.Domain.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Order.API.Application.Commands.Handlers
{
    public class OrderCommandHandler : CommandHandler, IRequestHandler<CreateOrderCommand, ValidationResult>
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBus _bus;
        public OrderCommandHandler(IVoucherRepository voucherRepository, 
                                    IOrderRepository orderRepository,
                                    IMessageBus bus)
        {
            _voucherRepository = voucherRepository;
            _orderRepository = orderRepository;
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;
            var order = MapOrder(message);
            if (!await ApplyVoucher(message, order)) return ValidationResult;
            if (!ValidateOrder(order)) return ValidationResult;
            if (!await ProcessPayment(order, message)) return ValidationResult;
            order.AuthorizeOrder();
            order.AddEvent(new FinishedOrderEvent(order.Id,order.CustomerId));
            _orderRepository.AddOrder(order);
            return await PersistData(_orderRepository.UnitOfWork);
        }

        private Domain.Entities.Order MapOrder(CreateOrderCommand message)
        {
            var address = new Address
            {
                Street = message.Address.Street,
                Number = message.Address.Number,
                Complement = message.Address.Complement,
                City = message.Address.City,
                Neighborhood = message.Address.Neighborhood,
                State = message.Address.State,
                ZipCode = message.Address.ZipCode
            };

            var order = new Domain.Entities.Order(message.CustomerId, message.TotalValue, message.Items.Select(OrderItemDTO.ToOrderItem).ToList(),
                message.VoucherApplyed, message.Discount);

            order.AddAddress(address);
            return order;
        }
        private async Task<bool> ApplyVoucher(CreateOrderCommand message, Domain.Entities.Order order)
        {
            if (!message.VoucherApplyed) return true;
            var voucher = await _voucherRepository.GetVoucherByCode(message.VoucherCode);
            if(voucher is null)
            {
                AddError("O voucher informado não existe!");
                return false;
            }
            var voucherValidation = new VoucherValidation().Validate(voucher);
            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(x => AddError(x.ErrorMessage));
                return false;
            }
            order.AddVoucher(voucher);
            voucher.DebitAmount();
            _voucherRepository.Update(voucher);
            return true;
        }
        private bool ValidateOrder(Domain.Entities.Order order)
        {
            var originalOrderValue = order.TotalValue;
            var originalOrderDiscount = order.Discount;
            order.CalculateOrderValue();

            if(order.TotalValue != originalOrderValue)
            {
                AddError("O valor total do pedido não confere com o total do pedido");
                return false;
            }
            if(order.Discount != originalOrderDiscount)
            {
                AddError("O valor do desconto não confere com o cálculo do pedido");
                return false;
            }
            return true;
        }
        private async Task<bool> ProcessPayment(Domain.Entities.Order order, CreateOrderCommand message)
        {
            var beganOrder = new BeganOrderIntegrationEvent
            {
                CustomerId = order.CustomerId,
                OrderId = order.Id,
                TotalValue = order.TotalValue,
                PaymentType = 1,
                CardOwnerName = message.CardOwnerName,
                CardCvv = message.CardCvv,
                CardExpirationDate = message.CardExpirationDate,
                CardNumber = message.CardNumber
            };

            var result = await _bus
                .RequestAsync<BeganOrderIntegrationEvent, ResponseMessage>(beganOrder);

            if (result.ValidationResult.IsValid) return true;

            foreach (var error in result.ValidationResult.Errors)
                AddError(error.ErrorMessage);
            
            return false;
        }
    }
}
