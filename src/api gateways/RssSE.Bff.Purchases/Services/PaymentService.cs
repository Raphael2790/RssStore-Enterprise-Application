using Microsoft.Extensions.Options;
using RssSE.Bff.Purchases.Extensions;
using RssSE.Bff.Purchases.Services.Interfaces;
using System;
using System.Net.Http;

namespace RssSE.Bff.Purchases.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _client;
        public PaymentService(HttpClient client, IOptions<AppServicesSettings> options)
        {
            _client = client;
            _client.BaseAddress = new Uri(options.Value.PaymentAPIUrl);
        }
    }
}
