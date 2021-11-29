using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        private readonly IPurchasesBffService _purchasesBffServive;

        public CartController(IPurchasesBffService purchasesBffServive)
        {
            _purchasesBffServive = purchasesBffServive;
        }

        [HttpGet("carrinho")]
        public async Task<IActionResult> Index() => View(await _purchasesBffServive.GetCart());

        [HttpPost("carrinho/adicionar-item")]
        public async Task<IActionResult> AddItemCart(CartItemViewModel productItem)
        {
            var response = await _purchasesBffServive.AddItemInCart(productItem);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _purchasesBffServive.GetCart());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("carrinho/atualizar-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            var item = new CartItemViewModel { ProductId = productId, Quantity = quantity };
            var response = await _purchasesBffServive.UpdateItemInCart(productId, item);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _purchasesBffServive.GetCart());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("carrinho/remover-item")]
        public async Task<IActionResult> RemoveItemCart(Guid productId)
        {
            var response = await _purchasesBffServive.RemoveItemInCart(productId);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _purchasesBffServive.GetCart());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("carrinho/aplicar-voucher")]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {
            var response = await _purchasesBffServive.ApplyVoucherCart(voucherCode);
            if (ResponseHasErrors(response)) return View(nameof(Index), await _purchasesBffServive.GetCart());
            return RedirectToAction(nameof(Index));
        }
    }
}
