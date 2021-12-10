using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RssSE.Core.Communication;
using RssSE.WebApi.Core.User.Interfaces;
using RssSE.WebApp.MVC.Interfaces.Services;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Base;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class IdentityService : HttpBaseService, IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAspNetUser _aspNetUser;

        public IdentityService(HttpClient httpClient, 
                                IAuthenticationService authenticationService, 
                                IAspNetUser aspNetUser)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _aspNetUser = aspNetUser;
        }

        public async Task<UserLoginResponse> Login(UserLoginViewModel userLogin)
        {
            var loginContent = GetContent(userLogin);
            var response = await _httpClient.PostAsync($"/api/v1/auth/autenticar", loginContent);
            if (!HasResponseError(response))
                return new UserLoginResponse
                {
                    ResponseResult = await DeserializeResponse<ResponseResult>(response)
                };
            return await DeserializeResponse<UserLoginResponse>(response);
        }

        public async Task<UserLoginResponse> Register(RegisterUserViewModel registerUser)
        {
            var registerContent = GetContent(registerUser);
            var response = await _httpClient.PostAsync($"/api/v1/auth/nova-conta", registerContent);
            if (!HasResponseError(response))
                return new UserLoginResponse
                {
                    ResponseResult = await DeserializeResponse<ResponseResult>(response)
                };
            return await DeserializeResponse<UserLoginResponse>(response);
        }

        public async Task<UserLoginResponse> UseRefreshToken(string refreshToken)
        {
            var content = GetContent(refreshToken);
            var response = await _httpClient.PostAsync("/api/v1/auth/refresh-token", content);
            if (!HasResponseError(response))
                return new UserLoginResponse
                {
                    ResponseResult = await DeserializeResponse<ResponseResult>(response)
                };
            return await DeserializeResponse<UserLoginResponse>(response);
        }

        public async Task ContextLogout()
        {
            await _authenticationService.SignOutAsync(
                _aspNetUser.GetHttpContext(), 
                CookieAuthenticationDefaults.AuthenticationScheme, 
                null);
        }

        public async Task ContextLogin(UserLoginResponse loginResponse)
        {
            var token = GetFormatedToken(loginResponse.AccessToken);

            var claims = new List<Claim>
            {
                new Claim("JWT", loginResponse.AccessToken),
                new Claim("RefreshToken", loginResponse.RefreshToken)
            };
            claims.AddRange(token.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties 
            { 
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8), 
                IsPersistent = true 
            };
            await _authenticationService.SignInAsync(
                _aspNetUser.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public static JwtSecurityToken GetFormatedToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token) as JwtSecurityToken;
        }

        public bool TokenHasExpired()
        {
            var jwt = _aspNetUser.GetUserToken();
            if (jwt is null) return false;
            var token = GetFormatedToken(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenIsValid()
        {
            var response = await UseRefreshToken(_aspNetUser.GetRefreshToken());
            if(response.AccessToken != null && response.ResponseResult is null)
            {
                await ContextLogin(response);
                return true;
            }

            return false;
        }
    }
}
