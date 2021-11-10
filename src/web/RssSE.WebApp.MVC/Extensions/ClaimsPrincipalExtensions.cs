using System;
using System.Security.Claims;

namespace RssSE.WebApp.MVC.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("sub");
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("email");
            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.FindFirst("JWT");
            return claim?.Value;
        }
    }
}
