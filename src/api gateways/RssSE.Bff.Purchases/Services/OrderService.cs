using Microsoft.Extensions.Options;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Models;
using RssSE.Bff.Purchases.Services.BaseService;
using RssSE.Bff.Purchases.Services.Interfaces;
using RssSE.Core.Communication;
using System;
using System.Collections.Generic;
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

        public async Task<ResponseResult> FinishOrder(OrderDTO order)
        {
            var orderContent = GetContent(order);

            var response = await _client.PostAsync("/pedido/", orderContent);

            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);

            return OkReturn();
        }

        public async Task<OrderDTO> GetLastOrder()
        {
            var response = await _client.GetAsync("/pedido/ultimo/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HasResponseError(response);

            return await DeserializeResponse<OrderDTO>(response);
        }

        public async Task<IEnumerable<OrderDTO>> GetListByCustomerId()
        {
            var response = await _client.GetAsync("/pedido/lista-cliente/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HasResponseError(response);

            return await DeserializeResponse<IEnumerable<OrderDTO>>(response);
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
