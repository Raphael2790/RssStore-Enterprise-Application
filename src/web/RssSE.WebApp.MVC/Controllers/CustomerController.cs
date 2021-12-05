using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    [Authorize]
    public class CustomerController : MainController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> NewAddress(AddressViewModel address)
        {
            var response = await _customerService.AddAddress(address);

            if (ResponseHasErrors(response)) TempData["Errors"] =
                     ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();

            return RedirectToAction("DeliveryAddress", "Order");
        }
    }
}
