using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Client.API.Application.Commands;
using RssSE.Client.API.Application.Events;
using RssSE.Client.API.Application.Handlers;
using RssSE.Client.API.Data.Context;
using RssSE.Client.API.Data.Repositories;
using RssSE.Client.API.Models.Interfaces;
using RssSE.Client.API.Services;
using RssSE.Core.Mediator;

namespace RssSE.Client.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, ClientCommandHandler>();
            services.AddScoped<INotificationHandler<RegisteredClientEvent>, ClientEventHandler>();
            services.AddScoped<ClientDbContext>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddHostedService<RegisterCustomerIntegrationHandler>();
        }
    }
}
