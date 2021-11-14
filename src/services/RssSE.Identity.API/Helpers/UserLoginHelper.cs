using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RssSE.Identity.API.Models;
using RssSE.WebApi.Core.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RssSE.Identity.API.Helpers
{
    public class UserLoginHelper : IUserLoginHelper
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly ClaimsIdentity _claimsIdentity;
        private readonly AppSettings _appSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public UserLoginHelper(IOptions<AppSettings> appSettings, UserManager<IdentityUser> userManager)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _claimsIdentity = new ClaimsIdentity();
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }

        public async Task<UserLoginResponse> GenerateUserToken(string email)
        {
            var userInfos = await GetUserInfos(email);
            return new UserLoginResponse
            {
                AccessToken = WriteUserToken(),
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationInHours).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = userInfos.User.Id,
                    Email = userInfos.User.Email,
                    UserClaims = userInfos.Claims.Select(x => new UserClaim { Type = x.Type, Value = x.Value })
                }
            };
        }

        private void GetUserClaimsIdentity(UserInfosDto userInfos)
        {
            userInfos.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userInfos.User.Id));
            userInfos.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, userInfos.User.Email));
            userInfos.Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userInfos.Claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            userInfos.Claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));

            foreach (var role in userInfos.Roles)
            {
                userInfos.Claims.Add(new Claim("role", role));
            }

            _claimsIdentity.AddClaims(userInfos.Claims);
        }

        private string WriteUserToken()
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = _tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.EmmitedBy,
                Audience = _appSettings.ValidFor,
                Subject = _claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            return _tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date) =>
           (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<UserInfosDto> GetUserInfos(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var userInfos = new UserInfosDto {User = user, Claims = userClaims, Roles = userRoles };
            GetUserClaimsIdentity(userInfos);
            return userInfos;
        }
    }
}
