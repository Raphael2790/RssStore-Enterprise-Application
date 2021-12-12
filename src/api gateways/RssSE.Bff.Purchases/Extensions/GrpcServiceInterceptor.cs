using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RssSE.Bff.Purchases.Extensions
{
    public class GrpcServiceInterceptor : Interceptor
    {
        //Utilizado o acesso ao invés do AspNetUser pq o ciclo de vida de um interceptor é singleton
        //E o httpcontext também
        private readonly ILogger<GrpcServiceInterceptor> _logger;
        private readonly IHttpContextAccessor _accessor;

        public GrpcServiceInterceptor(
            ILogger<GrpcServiceInterceptor> logger, 
            IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request, 
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var token = _accessor.HttpContext.Request.Headers["Authorization"];
            var headers = new Metadata
            {
                {"Authorization", token}
            };
            var options = context.Options.WithHeaders(headers);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
