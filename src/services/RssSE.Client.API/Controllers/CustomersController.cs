using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Client.API.Application.Commands;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Core.Mediator;
using RssSE.WebApi.Core.Controllers;
using RssSE.WebApi.Core.User.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssSE.Client.API.Controllers
{
    public class CustomersController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAspNetUser _user;

        public CustomersController(IMediatorHandler mediator, ICustomerRepository customerRepository, IAspNetUser user)
        {
            _mediator = mediator;
            _customerRepository = customerRepository;
            _user = user;
        }

        [HttpGet("registrar-cliente")]
        public async Task<IActionResult> RegisterClient()
        {
            var result = await _mediator
                .SendCommand(new RegisterCustomerCommand(Guid.NewGuid(), "Raphael", "raphael@raphae.com", "51361775351"));
            return CustomResponse(result);
        }

        [HttpGet("cliente/endereco")]
        public async Task<IActionResult> GetAddres()
        {
            var address = await _customerRepository.GetAddresByCustomerId(_user.GetUserId());
            return address is null ? NotFound() : CustomResponse(address);
        }

        [HttpPost("cliente/endereco")]
        public async Task<IActionResult> AddAddress(CreateAddressCommand address)
        {
            address.SetCustomer(_user.GetUserId());
            return CustomResponse(await _mediator.SendCommand(address));
        }
    }
}
