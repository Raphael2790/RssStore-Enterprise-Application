using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Bff.Purchases.Models;
using RssSE.Bff.Purchases.Services.gRPC;
using RssSE.Bff.Purchases.Services.Interfaces;
using RssSE.WebApi.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        private readonly ICartService _cartService;
        private readonly ICartGrpcService _cartGrpcService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, 
                                ICatalogService catalogService, 
                                IOrderService orderService, 
                                ICartGrpcService cartGrpcService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
            _orderService = orderService;
            _cartGrpcService = cartGrpcService;
        }

        [HttpGet("compras/carrinho")]
        public async Task<IActionResult> Index() => CustomResponse(await _cartService.GetCart());

        [HttpGet("compras/carrinho-quantidade")]
        public async Task<int> GetCartItensQuantity()
        {
            var cart = await _cartService.GetCart();
            return cart?.CartItems.Sum(x => x.Quantity) ?? 0;
        }

        [HttpPost("compras/carrinho/items")]
        public async Task<IActionResult> AddCartItem(CartItemDTO cartItem)
        {
            var product = await _catalogService.GetById(cartItem.ProductId);
            await ValidateCartItem(product, cartItem.Quantity);
            if (!IsValidOperation()) return CustomResponse();
            cartItem.Image = product.Image;
            cartItem.UnitValue = product.Value;
            cartItem.Name = product.Name;
            var response = await _cartService.AddItemInCart(cartItem);
            return CustomResponse(response);
        }

        [HttpPut("compras/carrinho/items/{productId:guid}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItemDTO cartItem)
        {
            var product = await _catalogService.GetById(cartItem.ProductId);
            await ValidateCartItem(product, cartItem.Quantity);
            if (!IsValidOperation()) return CustomResponse();
            var response = await _cartService.UpdateItemInCart(productId,cartItem);
            return CustomResponse(response);
        }

        [HttpDelete("compras/carrinho/items/{productId:guid}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var product = await _catalogService.GetById(productId);
            if(product is null)
            {
                AddProcessError("Produto inexistente!");
                return CustomResponse();
            }
            var response = await _cartService.RemoveItemInCart(productId);
            return CustomResponse(response);
        }

        [HttpPost("compras/carrinho/aplicar-voucher")]
        public async Task<IActionResult> ApplyVoucher([FromBody] string code)
        {
            var voucher = await _orderService.GetVoucherByCode(code);
            if(voucher is null)
            {
                AddProcessError("Voucher inválido ou inexistente");
                return CustomResponse();
            }
            var response = await _cartService.ApplyVoucherOnCart(voucher);
            return CustomResponse(response);
        } 
        
        private async Task ValidateCartItem(ItemProductDTO product, int quantity)
        {
            if (product is null) AddProcessError("Produto inexistente!");
            if (quantity < 1) AddProcessError($"Escolha ao menos uma unidade do produto {product.Name}");
            var cart = await _cartService.GetCart();
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == product.Id);
            if(cartItem != null && cartItem.Quantity + quantity > product.StockAmount)
            {
                AddProcessError($"O produto {product.Name} possui {product.StockAmount} unidades em estoque, voce selecionou {quantity + cartItem.Quantity}");
                return;
            }
            if (quantity > product.StockAmount) 
                AddProcessError($"O produto {product.Name} possui apenas {product.StockAmount} unidades em estoque, voce selecionou {quantity} unidades");
        }

    }
}
