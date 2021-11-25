using Microsoft.Extensions.Options;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Models;
using RssSE.Bff.Purchases.Services.BaseService;
using RssSE.Bff.Purchases.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services
{
    public class CatalogService : HttpBaseService, ICatalogService
    {
        private readonly HttpClient _client;
        public CatalogService(HttpClient client, IOptions<AppServicesSettings> options)
        {
            _client = client;
            _client.BaseAddress = new Uri(options.Value.CatalogAPIUrl);
        }

        public async Task<ItemProductDTO> GetById(Guid productId)
        {
            var response = await _client.GetAsync($"/catalogo/produto/{productId}");
            HasResponseError(response);
            return await DeserializeResponse<ItemProductDTO>(response);
        }
    }
}
