using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;

namespace RssSE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            if (IsMappedStatusCodeError(id)) 
            {
                var modelError = ErrorViewModel.ErroViewModelFactory.CreateErrorViewModel(id);
                return View("Error", modelError);
            }
            else
            {
                return StatusCode(404);
            }
        }

        private bool IsMappedStatusCodeError(int id) => id == 500 || id == 404 || id == 403;
    }
}
