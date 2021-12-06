using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
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

        [HttpGet("pagamento")]
        public async Task<IActionResult> Payment()
        {
            var cart = await _purchasesBffService.GetCart();
            if (cart.CartItems.Count == 0) return RedirectToAction("Index", "Cart");
            var order = _purchasesBffService.MapToOrder(cart, null);
            return View(order);
        }

        [HttpPost("finalizar-pedido")]
        public async Task<IActionResult> FinishOrder(OrderTransactionViewModel orderTransaction)
        {
            if (!ModelState.IsValid) 
                return View("Payment", _purchasesBffService.MapToOrder(await _purchasesBffService.GetCart(), null));
            var response = await _purchasesBffService.FinishOrder(orderTransaction);
            if (ResponseHasErrors(response))
            {
                var cart = await _purchasesBffService.GetCart();
                if (cart.CartItems.Count == 0) return View("Index", "Cart");
                var orderMap = _purchasesBffService.MapToOrder(cart, null);
                return View("Payment", orderMap);
            }

            return RedirectToAction("FinishedOrder");
        }

        [HttpGet("pedido-concluido")]
        public async Task<IActionResult> FinishedOrder()
        {
            var order = await _purchasesBffService.GetLastOrder();
            return View("OrderConfirmation", order);
        }

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _purchasesBffService.GetListByCustomerId());
        }
    }
}
