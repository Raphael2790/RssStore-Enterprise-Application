using Microsoft.AspNetCore.Identity;
using RssSE.Identity.API.Models;
using RssSE.Identity.Data.Entities;
using System;
using System.Threading.Tasks;

namespace RssSE.Identity.API.Helpers
{
    public interface IAuthenticationService
    {
        UserManager<IdentityUser> UserManager { get;}
        SignInManager<IdentityUser> SignInManager { get;}
        Task<UserLoginResponse> GenerateUserToken(string email);
        Task<RefreshToken> GetRefreshToken(Guid refreshToken);
    }
}
