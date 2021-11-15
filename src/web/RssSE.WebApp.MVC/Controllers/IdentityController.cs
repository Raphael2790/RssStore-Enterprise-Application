using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Interfaces.Services;
using RssSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            await LoginUserInContext(response);
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
            await LoginUserInContext(response);
            if(string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");
            return LocalRedirect(returnUrl);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Catalog");
        }

        private async Task LoginUserInContext(UserLoginResponse response)
        {
            var token = GetFormatedToken(response.AccessToken);
            var claims = new List<Claim>
            {
                new Claim("JWT", response.AccessToken)
            };
            claims.AddRange(token.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60), IsPersistent = true };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private static JwtSecurityToken GetFormatedToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token) as JwtSecurityToken;
        }
    }
}
