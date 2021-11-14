using RssSE.WebApp.MVC.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services.Base
{
    public abstract class HttpBaseService
    {
        private readonly JsonSerializerOptions _options;
        public HttpBaseService()
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        protected bool HasResponseError(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);
                case 400:
                    return false;
            }
            response.EnsureSuccessStatusCode();
            return true;
        }

        protected StringContent GetContent(object obj) =>
            new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                "application/json");

        protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage) =>
             JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), _options);
    }
}
