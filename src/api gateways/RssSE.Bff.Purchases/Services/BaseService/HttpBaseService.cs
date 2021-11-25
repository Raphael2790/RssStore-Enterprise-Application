using RssSE.Core.Communication;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.BaseService
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
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;
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

        protected ResponseResult OkReturn() => new ResponseResult();
    }
}
