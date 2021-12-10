using System;
using System.Security.Claims;

namespace RssSE.WebApi.Core.User.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("sub");
            if (claim is null)
                claim = claimsPrincipal.FindFirst(c => c.Type.Contains("nameidentifier"));
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("email");
            if (claim is null)
                claim = claimsPrincipal.FindFirst(c => c.Type.Contains("emailaddress"));
            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("JWT");
            return claim?.Value;
        }

        public static string GetRefreshToken(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("RefreshToken");
            return claim?.Value;
        }
    }
}
