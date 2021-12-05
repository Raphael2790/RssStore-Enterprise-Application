using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Core.Mediator;
using RssSE.Order.API.Application.Commands;
using RssSE.Order.API.Application.Queries;
using RssSE.WebApi.Core.Controllers;
using RssSE.WebApi.Core.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Order.API.Controllers
{
    [Authorize]
    public class OrderController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IOrderQueries _orderQueries;
        private readonly IAspNetUser _user;

        public OrderController(IMediatorHandler mediator, 
                                IOrderQueries orderQueries, 
                                IAspNetUser user)
        {
            _mediator = mediator;
            _orderQueries = orderQueries;
            _user = user;
        }

        [HttpPost("pedido")]
        public async Task<IActionResult> AddOrder(CreateOrderCommand pedido)
        {
            pedido.CustomerId = _user.GetUserId();
            return CustomResponse(await _mediator.SendCommand(pedido));
        }

        [HttpGet("pedido/ultimo")]
        public async Task<IActionResult> LastOrder()
        {
            var order = await _orderQueries.GetLastOrder(_user.GetUserId());
            return order is null ? NotFound() : CustomResponse(order);
        }

        [HttpGet("pedido/list-cliente")]
        public async Task<IActionResult> ListByCustomer()
        {
            var orders = await _orderQueries.GetListByCustomer(_user.GetUserId());
            return orders is null || !orders.Any() ? NotFound() : CustomResponse(orders);
        }
    }
}
