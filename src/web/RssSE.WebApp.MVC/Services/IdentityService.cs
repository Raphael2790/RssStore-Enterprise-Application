using RssSE.WebApp.MVC.Interfaces.Services;
using RssSE.WebApp.MVC.Models;
using RssSE.WebApp.MVC.Services.Base;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class IdentityService : HttpBaseService, IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            if (HasResponseError(response))
                return new UserLoginResponse
                {
                    ResponseResult = await DeserializeResponse<ResponseResult>(response)
                };
            return await DeserializeResponse<UserLoginResponse>(response);
        }
    }
}
