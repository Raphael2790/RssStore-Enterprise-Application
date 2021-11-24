using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [HttpGet("carrinho")]
        public async Task<IActionResult> Index() => View(await _cartService.GetCart());

        [HttpPost("carrinho/adicionar-item")]
        public async Task<IActionResult> AddItemCart(ProductItemViewModel productItem)
        {
            var product = await _catalogService.Get(productItem.ProductId);
            ValidateCartItem(product, productItem.Quantity);
            productItem.Name = product.Name;
            productItem.Image = product.Image;
            productItem.UnitValue = product.Value;
            if (!IsValidOperation()) return View(nameof(Index), await _cartService.GetCart());
            var response = await _cartService.AddItemInCart(productItem);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _cartService.GetCart());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("carrinho/atualizar-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            var product = await _catalogService.Get(productId);
            ValidateCartItem(product, quantity);
            if (!IsValidOperation()) return View(nameof(Index), await _cartService.GetCart());
            var productItem = new ProductItemViewModel { ProductId = productId, Quantity = quantity };
            var response = await _cartService.UpdateItemInCart(productId, productItem);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _cartService.GetCart());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("carrinho/remover-item")]
        public async Task<IActionResult> RemoveItemCart(Guid productId)
        {
            var product = await _catalogService.Get(productId);
            if(product is null)
            {
                AddValidationError("Produto inexistente!");
                return View(nameof(Index), await _cartService.GetCart());
            }
            var response = await _cartService.RemoveItemInCart(productId);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _cartService.GetCart());
            return RedirectToAction(nameof(Index));
        }

        private void ValidateCartItem(ProductViewModel product, int quantity)
        {
            if(product is null) AddValidationError("Produto inexistente!");
            if(quantity <= 0) AddValidationError($"Escolha ao menos uma unidade do produto {product.Name}");
            if (quantity > product.StockAmount) AddValidationError($"O produto {product.Name} possui apenas {product.StockAmount} em estoque");
        }
    }
}
