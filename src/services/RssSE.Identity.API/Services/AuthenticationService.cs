using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Interfaces;
using RssSE.Identity.API.Extensions;
using RssSE.Identity.API.Models;
using RssSE.Identity.Data.Context;
using RssSE.Identity.Data.Entities;
using RssSE.WebApi.Core.Identity;
using RssSE.WebApi.Core.User.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RssSE.Identity.API.Helpers
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly ClaimsIdentity _claimsIdentity;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly AppTokenSettings _appTokenSettings;

        private readonly IAspNetUser _aspNetUser;
        private readonly IJsonWebKeySetService _jwksService;

        public UserManager<IdentityUser> UserManager => _userManager;

        public SignInManager<IdentityUser> SignInManager => _signInManager;

        public AuthenticationService(UserManager<IdentityUser> userManager,
                                    IJsonWebKeySetService jwksService,
                                    ApplicationDbContext context,
                                    SignInManager<IdentityUser> signInManager, 
                                    IOptions<AppTokenSettings> appTokenSettings)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _claimsIdentity = new ClaimsIdentity();
            _userManager = userManager;
            _jwksService = jwksService;
            _context = context;
            _signInManager = signInManager;
            _appTokenSettings = appTokenSettings.Value;
        }

        public async Task<UserLoginResponse> GenerateUserToken(string email)
        {
            var userInfos = await GetUserInfos(email);
            return new UserLoginResponse
            {
                AccessToken = WriteUserToken(),
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
                RefreshToken = (await GenerateRefreshToken(email)).Token,
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
            var key = _jwksService.GetCurrentSigningCredentials();
            var currentIssuer = $"{_aspNetUser.GetHttpContext().Request.Scheme}://{_aspNetUser.GetHttpContext().Request.Host}";
            var token = _tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = _claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
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

        private async Task<RefreshToken> GenerateRefreshToken(string userEmail)
        {
            var refreshToken = new RefreshToken
            {
                UserName = userEmail,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettings.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.UserName == userEmail));
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken> GetRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            return !(token is null) && token.ExpirationDate.ToLocalTime() > DateTime.Now 
                ? token
                : null;
        }
    }
}
