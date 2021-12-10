using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using Refit;
using RssSE.WebApp.MVC.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static IIdentityService _identityService;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IIdentityService identityService)
        {
            _identityService = identityService;

            try
            {
                await _next(context);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(context, ex.StatusCode);
            }
            catch(ValidationApiException ex)
            {
                HandleRequestExceptionAsync(context, ex.StatusCode);
            }
            catch(ApiException ex)
            {
                HandleRequestExceptionAsync(context, ex.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitExceptionAsync(context);
            }
        }

        private void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if(statusCode == HttpStatusCode.Unauthorized)
            {
                if (_identityService.TokenHasExpired())
                {
                    if (_identityService.RefreshTokenIsValid().Result)
                    {
                        context.Response.Redirect(context.Request.Path);
                        return;
                    }
                }

                _identityService.ContextLogout();
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private void HandleBrokenCircuitExceptionAsync(HttpContext context) => context.Response.Redirect("/sistema-indisponível");
    }
}
