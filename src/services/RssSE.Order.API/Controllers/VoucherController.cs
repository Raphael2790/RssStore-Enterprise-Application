using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Order.API.Application.DTOs;
using RssSE.Order.API.Application.Queries;
using RssSE.WebApi.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RssSE.Order.API.Controllers
{
    [Authorize]
    public class VoucherController : MainController
    {
        private readonly IVoucherQueries _voucherQuery;

        public VoucherController(IVoucherQueries voucherQuery)
        {
            _voucherQuery = voucherQuery;
        }

        [HttpGet("voucher/{code}")]
        [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return NotFound();
            var voucher = await _voucherQuery.GetVoucherByCode(code);
            return voucher is null ? NotFound() : CustomResponse(voucher);
        }
    }
}
