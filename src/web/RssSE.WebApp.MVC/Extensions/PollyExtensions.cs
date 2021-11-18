using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace RssSE.WebApp.MVC.Extensions
{
    public class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> RetryAsyncWithThreeAttemptsAndLogging()
        {
            return  HttpPolicyExtensions
                            .HandleTransientHttpError()
                            .WaitAndRetryAsync(new[]
                            {
                                TimeSpan.FromSeconds(2),
                                TimeSpan.FromSeconds(4),
                                TimeSpan.FromSeconds(12)
                            }, (outcome, timespan, retryCount, context) =>
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine($"Tentando pela {retryCount} vez!");
                                Console.ForegroundColor = ConsoleColor.White;
                            });
        }

        public static AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakAfterThreeAttempts()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
