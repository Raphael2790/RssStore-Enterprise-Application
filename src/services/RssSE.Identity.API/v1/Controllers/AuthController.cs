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
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageBus _bus;

        public AuthController(IAuthenticationService userLoginHelper, IMessageBus bus)
        {
            _authenticationService = userLoginHelper;
            _bus = bus;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };
            var result = await _authenticationService.UserManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var customerRegistrationResult = await RegisterCustomer(registerUser);

                if (!customerRegistrationResult.ValidationResult.IsValid)
                {
                    await _authenticationService.UserManager.DeleteAsync(user);
                    return CustomResponse(customerRegistrationResult.ValidationResult);
                }

                return CustomResponse(await _authenticationService.GenerateUserToken(registerUser.Email));
            }

            AddProcessErrors(result.Errors.Select(x => x.Description));

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _authenticationService.SignInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);
            if (result.Succeeded) return CustomResponse(await _authenticationService.GenerateUserToken(userLogin.Email));
            if (result.IsLockedOut)
            {
                AddProcessError("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }
            AddProcessError("Usuário ou senha inválido");
            return CustomResponse();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AddProcessError("Refresh token inválido");
                return CustomResponse();
            }

            var token = await _authenticationService.GetRefreshToken(Guid.Parse(refreshToken));

            if(token is null)
            {
                AddProcessError("Refresh token expirado");
            }

            return CustomResponse(_authenticationService.GenerateUserToken(token.UserName));
        }

        private async Task<ResponseMessage> RegisterCustomer(RegisterUserViewModel registerUser)
        {
            var user = await _authenticationService.UserManager.FindByEmailAsync(registerUser.Email);
            var registerCustomer = new RegisteredUserIntegrationEvent(Guid.Parse(user.Id), registerUser.Name, registerUser.Email,
                registerUser.Cpf);
            try
            {
                var success = await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registerCustomer);
                return success;
            }
            catch
            {
                await _authenticationService.UserManager.DeleteAsync(user);
                throw;
            }
        }
    }
}
