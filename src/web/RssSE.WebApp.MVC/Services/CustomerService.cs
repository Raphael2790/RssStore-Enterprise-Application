using RssSE.Core.Communication;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Base;
using RssSE.WebApp.MVC.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class CustomerService : HttpBaseService ,ICustomerService
    {
        private readonly HttpClient _client;

        public CustomerService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseResult> AddAddress(AddressViewModel address)
        {
            var content = GetContent(address);
            var response = await _client.PostAsync("cliente/endereco", content);
            if(!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<AddressViewModel> GetAddress()
        {
            var response = await _client.GetAsync("cliente/endereco");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            HasResponseError(response);
            return await DeserializeResponse<AddressViewModel>(response);
        }
    }
}
