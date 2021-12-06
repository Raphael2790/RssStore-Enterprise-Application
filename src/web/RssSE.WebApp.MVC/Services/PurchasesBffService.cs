using RssSE.Core.Communication;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Base;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class PurchasesBffService : HttpBaseService, IPurchasesBffService
    {
        private readonly HttpClient _client;
        public PurchasesBffService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseResult> AddItemInCart(CartItemViewModel productItem)
        {
            var itemContent = GetContent(productItem);
            var response = await _client.PostAsync("compras/carrinho/items", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<ResponseResult> ApplyVoucherCart(string voucher)
        {
            var itemContent = GetContent(voucher);
            var response = await _client.PostAsync("compras/carrinho/aplicar-voucher", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<ResponseResult> FinishOrder(OrderTransactionViewModel orderTransaction)
        {
            var content = GetContent(orderTransaction);
            var response = await _client.PostAsync("compras/pedido", content);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _client.GetAsync("compras/carrinho");
            HasResponseError(response);
            return await DeserializeResponse<CartViewModel>(response);
        }

        public async Task<int> GetCartItemsAmount()
        {
            var response = await _client.GetAsync("/compras/carrinho-quantidade");
            HasResponseError(response);
            return await DeserializeResponse<int>(response);
        }

        public async Task<OrderViewModel> GetLastOrder()
        {
            var response = await _client.GetAsync("/compras/pedido/ultimo");
            HasResponseError(response);
            return await DeserializeResponse<OrderViewModel>(response);
        }

        public async Task<IEnumerable<OrderViewModel>> GetListByCustomerId()
        {
            var response = await _client.GetAsync("/compras/pedido/lista-cliente");
            HasResponseError(response);
            return await DeserializeResponse<IEnumerable<OrderViewModel>>(response);
        }

        public OrderTransactionViewModel MapToOrder(CartViewModel cart, AddressViewModel address)
        {
            var order = new OrderTransactionViewModel
            {
                TotalValue = cart.TotalValue,
                VoucherCode = cart.Voucher?.Code,
                Items = cart.CartItems,
                VoucherApplyed = cart.VoucherApplyed,
                Discount = cart.Discount
            };

            if(address != null)
            {
                order.Address = new AddressViewModel(address.Street, address.Number, address.Complement, address.Neighborhood
                    , address.ZipCode, address.City, address.State);
            }

            return order;
        }

        public async Task<ResponseResult> RemoveItemInCart(Guid productId)
        {
            var response = await _client.DeleteAsync($"compras/carrinho/items/{productId}");
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }

        public async Task<ResponseResult> UpdateItemInCart(Guid productId, CartItemViewModel productItem)
        {
            var itemContent = GetContent(productItem);
            var response = await _client.PutAsync($"compras/carrinho/items/{productId}", itemContent);
            if (!HasResponseError(response)) return await DeserializeResponse<ResponseResult>(response);
            return OkReturn();
        }
    }
}
