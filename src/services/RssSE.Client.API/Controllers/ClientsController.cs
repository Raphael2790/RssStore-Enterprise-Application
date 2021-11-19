using Microsoft.AspNetCore.Mvc;
using RssSE.Client.API.Application.Commands;
using RssSE.Core.Mediator;
using RssSE.WebApi.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Client.API.Controllers
{
    public class ClientsController : MainController
    {
        private readonly IMediatorHandler _mediator;

        public ClientsController(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("registrar-cliente")]
        public async Task<IActionResult> RegisterClient()
        {
            var result = await _mediator
                .SendCommand(new RegisterCustomerCommand(Guid.NewGuid(), "Raphael", "raphael@raphae.com", "51361775351"));
            return CustomResponse(result);
        }
    }
}
