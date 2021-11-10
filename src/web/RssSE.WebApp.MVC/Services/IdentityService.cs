using RssSE.WebApp.MVC.Interfaces.Services;
using RssSE.WebApp.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<UserLoginResponse> Login(UserLoginViewModel userLogin)
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(userLogin),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"/api/v1/auth/autenticar", loginContent);
            return JsonSerializer.Deserialize<UserLoginResponse>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<UserLoginResponse> Register(RegisterUserViewModel registerUser)
        {
            var registerContent = new StringContent(
                JsonSerializer.Serialize(registerUser),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"/api/v1/auth/nova-conta", registerContent);
            return JsonSerializer.Deserialize<UserLoginResponse>(await response.Content.ReadAsStringAsync(), _options);
        }
    }
}
