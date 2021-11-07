using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RssSE.Identity.API.Models;
using System.Threading.Tasks;

namespace RssSE.Identity.API.v1.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UserLoginViewModel userLogin)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);
            if (result.Succeeded) return Ok();
            return BadRequest();
        }
    }
}
