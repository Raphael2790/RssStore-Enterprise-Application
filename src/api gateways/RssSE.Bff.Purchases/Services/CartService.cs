using Microsoft.Extensions.Options;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Models;
using RssSE.Bff.Purchases.Services.BaseService;
using RssSE.Bff.Purchases.Services.Interfaces;
using RssSE.Core.Communication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services
{
    public class CartService : HttpBaseService, ICartService
    {
        private readonly HttpClient _client;
        public CartService(HttpClient client, IOptions<AppServicesSettings> options)
        {
            _client = client;
            _client.BaseAddress = new Uri(options.Value.CartAPIUrl);
        }

        public async Task<ResponseResult> AddItemInCart(CartItemDTO cartItem)
        {
            var itemContent = GetContent(cartItem);
            var response = await _client.PostAsync("/carrinho", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _client.GetAsync("/carrinho");
            HasResponseError(response);
            return await DeserializeResponse<CartDTO>(response);
        }

        public async Task<ResponseResult> RemoveItemInCart(Guid productId)
        {
            var response = await _client.DeleteAsync($"carrinho/{productId}");
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<ResponseResult> UpdateItemInCart(Guid productId, CartItemDTO cartItem)
        {
            var itemContent = GetContent(cartItem);
            var response = await _client.PutAsync($"carrinho/{productId}", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }
    }
}
