using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Services.Interfaces;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPurchasesBffService _purchasesBffService;
        public CartViewComponent(IPurchasesBffService purchasesBffService)
        {
            _purchasesBffService = purchasesBffService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _purchasesBffService.GetCartItemsAmount());
        }
    }
}
