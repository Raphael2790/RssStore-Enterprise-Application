using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RssSE.Core.Messages.Integration;
using RssSE.Identity.API.Helpers;
using RssSE.Identity.API.Models;
using RssSE.MessageBus;
using RssSE.WebApi.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Identity.API.v1.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserLoginHelper _userLoginHelper;
        private readonly IMessageBus _bus;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUserLoginHelper userLoginHelper, IMessageBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userLoginHelper = userLoginHelper;
            _bus = bus;
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
            {
                var customerRegistrationResult = await RegisterCustomer(registerUser);

                if (!customerRegistrationResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(customerRegistrationResult.ValidationResult);
                }

                return CustomResponse(await _userLoginHelper.GenerateUserToken(registerUser.Email));
            }

            AddProcessErrors(result.Errors.Select(x => x.Description));

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

        private async Task<ResponseMessage> RegisterCustomer(RegisterUserViewModel registerUser)
        {
            var user = await _userManager.FindByEmailAsync(registerUser.Email);
            var registerCustomer = new RegisteredUserIntegrationEvent(Guid.Parse(user.Id), registerUser.Name, registerUser.Email,
                registerUser.Cpf);
            try
            {
                var success = await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registerCustomer);
                return success;
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }
    }
}
