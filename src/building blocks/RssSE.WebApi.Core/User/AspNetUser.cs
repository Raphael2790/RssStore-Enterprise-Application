using Microsoft.AspNetCore.Http;
using RssSE.WebApi.Core.User.Extensions;
using RssSE.WebApi.Core.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RssSE.WebApi.Core.User
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _acessor;

        public AspNetUser(IHttpContextAccessor acessor)
        {
            _acessor = acessor;
        }
        public string Name => _acessor.HttpContext.User.Identity.Name;

        public IEnumerable<Claim> GetClaims() => _acessor.HttpContext.User.Claims;

        public HttpContext GetHttpContext() => _acessor.HttpContext;

        public string GetRefreshToken() => IsAuthenticated() ? _acessor.HttpContext.User.GetRefreshToken() : "";

        public string GetUserEmail() => IsAuthenticated() ? _acessor.HttpContext.User.GetUserEmail() : "";

        public Guid GetUserId() => IsAuthenticated() ? Guid.Parse(_acessor.HttpContext.User.GetUserId()) : Guid.Empty;

        public string GetUserToken() => IsAuthenticated() ? _acessor.HttpContext.User.GetUserToken() : "";

        public bool HasRole(string role) => _acessor.HttpContext.User.IsInRole(role);

        public bool IsAuthenticated() => _acessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
