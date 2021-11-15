using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Base;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class CatalogService : HttpBaseService, ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProductViewModel> Get(Guid id)
        {
            var response = await _client.GetAsync($"/catalogo/produto/{id}");
            HasResponseError(response);
            return await DeserializeResponse<ProductViewModel>(response);
        }

        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            var response = await _client.GetAsync("catalogo/produtos");
            HasResponseError(response);
            return await DeserializeResponse<IEnumerable<ProductViewModel>>(response);
        }
    }
}
