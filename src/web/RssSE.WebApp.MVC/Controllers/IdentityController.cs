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
    public class IdentityController : Controller
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

            //if (false) return View(registerUser);

            await LoginUserInContext(response);

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

            var response = await _identityService.Login(userLogin);

            //if (false) return View(userLogin);
            await LoginUserInContext(response);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        private async Task LoginUserInContext(UserLoginResponse response)
        {
            var token = GetFormatedToken(response.AccessToken);
            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
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
