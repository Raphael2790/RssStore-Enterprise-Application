using RssSE.Identity.API.Models;
using System.Threading.Tasks;

namespace RssSE.Identity.API.Helpers
{
    public interface IUserLoginHelper
    {
        Task<UserLoginResponse> GenerateUserToken(string email);
    }
}
