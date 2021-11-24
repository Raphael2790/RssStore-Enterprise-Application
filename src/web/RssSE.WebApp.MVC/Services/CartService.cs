using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Base;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class CartService : HttpBaseService, ICartService
    {
        private readonly HttpClient _client;
        public CartService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseResult> AddItemInCart(ProductItemViewModel productItem)
        {
            var itemContent = GetContent(productItem);
            var response = await _client.PostAsync("/carrinho", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _client.GetAsync("/carrinho");
            HasResponseError(response);
            return await DeserializeResponse<CartViewModel>(response);
        }

        public async Task<ResponseResult> RemoveItemInCart(Guid productId)
        {
            var response = await _client.DeleteAsync($"carrinho/{productId}");
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<ResponseResult> UpdateItemInCart(Guid productId, ProductItemViewModel productItem)
        {
            var itemContent = GetContent(productItem);
            var response = await _client.PutAsync($"carrinho/{productId}", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }
    }
}
