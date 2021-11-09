using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    public class IdentityController : Controller
    {
        [HttpGet("nova-conta")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return View(registerUser);

            if (false) return View(registerUser);

            return RedirectToAction("Index", nameof(HomeController));
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if (!ModelState.IsValid) return View(userLogin);

            if (false) return View(userLogin);

            return RedirectToAction("Index", nameof(HomeController));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", nameof(HomeController));
        }
    }
}
