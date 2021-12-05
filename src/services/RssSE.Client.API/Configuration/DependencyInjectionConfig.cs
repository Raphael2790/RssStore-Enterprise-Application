using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Client.API.Application.Commands;
using RssSE.Client.API.Application.Events;
using RssSE.Client.API.Application.Handlers;
using RssSE.Client.API.Data.Context;
using RssSE.Client.API.Data.Repositories;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Client.API.Services;
using RssSE.Core.Mediator;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Client.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<CreateAddressCommand, ValidationResult>, CustomerCommandHandler>();

            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();

            services.AddScoped<CustomerDbContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
