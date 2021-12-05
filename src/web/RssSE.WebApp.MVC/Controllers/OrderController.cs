using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Services.Interfaces;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{

    public class OrderController : MainController
    {
        private readonly ICustomerService _customerService;
        private readonly IPurchasesBffService _purchasesBffService;

        public OrderController(ICustomerService customerService, IPurchasesBffService purchasesBffService)
        {
            _customerService = customerService;
            _purchasesBffService = purchasesBffService;
        }

        [HttpGet("endereco-entrega")]
        public async Task<IActionResult> DeliveryAddress()
        {
            var cart = await _purchasesBffService.GetCart();
            if (cart.CartItems.Count == 0) return RedirectToAction("Index", "Cart");
            var address = await _customerService.GetAddress();
            var order = _purchasesBffService.MapToOrder(cart, address);
            return View(order);
        }
    }
}
