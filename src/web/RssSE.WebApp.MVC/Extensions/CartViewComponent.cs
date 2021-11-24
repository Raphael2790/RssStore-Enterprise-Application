using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Interfaces;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _cartService.GetCart() ?? new CartViewModel());
        }
    }
}
