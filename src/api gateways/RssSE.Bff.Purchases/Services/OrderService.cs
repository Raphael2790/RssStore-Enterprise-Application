using Microsoft.Extensions.Options;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Models;
using RssSE.Bff.Purchases.Services.BaseService;
using RssSE.Bff.Purchases.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services
{
    public class OrderService : HttpBaseService, IOrderService
    {
        private readonly HttpClient _client;
        public OrderService(HttpClient client, IOptions<AppServicesSettings> options)
        {
            _client = client;
            _client.BaseAddress = new Uri(options.Value.OrderAPIUrl);
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var response = await _client.GetAsync($"/voucher/{code}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            HasResponseError(response);
            return await DeserializeResponse<VoucherDTO>(response);
        }
    }
}
