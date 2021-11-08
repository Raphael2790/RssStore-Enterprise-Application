using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RssSE.Identity.API.Helpers;
using RssSE.Identity.API.Models;
using System.Threading.Tasks;

namespace RssSE.Identity.API.v1.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserLoginHelper _userLoginHelper;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUserLoginHelper userLoginHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userLoginHelper = userLoginHelper;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
                return CustomResponse(await _userLoginHelper.GenerateUserToken(registerUser.Email));

            foreach (var error in result.Errors)
                AddProcessError(error.Description);

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UserLoginViewModel userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);
            if (result.Succeeded) return CustomResponse(await _userLoginHelper.GenerateUserToken(userLogin.Email));
            if (result.IsLockedOut)
            {
                AddProcessError("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }
            AddProcessError("Usuário ou senha inválido");
            return CustomResponse();
        }
    }
}
