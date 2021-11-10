using RssSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<UserLoginResponse> Login(UserLoginViewModel userLogin);
        Task<UserLoginResponse> Register(RegisterUserViewModel registerUser);
    }
}
