using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Interfaces.Services;
using RssSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    public class IdentityController : MainController
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet("nova-conta")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return View(registerUser);
            var response = await _identityService.Register(registerUser);
            if (ResponseHasErrors(response.ResponseResult)) return View(registerUser);
            await _identityService.ContextLogin(response);
            return RedirectToAction("Index", nameof(HomeController));
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(userLogin);
            var response = await _identityService.Login(userLogin);
            if (ResponseHasErrors(response.ResponseResult)) return View(userLogin);
            await _identityService.ContextLogin(response);
            if(string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");
            return LocalRedirect(returnUrl);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _identityService.ContextLogout();
            return RedirectToAction("Index", "Catalog");
        }
    }
}
