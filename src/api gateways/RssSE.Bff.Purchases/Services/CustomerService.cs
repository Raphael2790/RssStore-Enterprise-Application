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
    public class CustomerService : HttpBaseService, ICustomerService
    {
        private readonly HttpClient _client;

        public CustomerService(HttpClient client, IOptions<AppServicesSettings> options)
        {
            _client = client;
            _client.BaseAddress = new Uri(options.Value.CustomerAPIUrl);
        }

        public async Task<AddressDTO> GetAddress()
        {
            var response = await _client.GetAsync("/cliente/endereco");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            HasResponseError(response);
            return await DeserializeResponse<AddressDTO>(response);
        }
    }
}
