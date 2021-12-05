using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RssSE.Core.Mediator;
using RssSE.Order.API.Application.Commands;
using RssSE.Order.API.Application.Commands.Handlers;
using RssSE.Order.API.Application.Events;
using RssSE.Order.API.Application.Events.Handlers;
using RssSE.Order.API.Application.Queries;
using RssSE.Order.Domain.Repositories;
using RssSE.Order.Infra.Data.Context;
using RssSE.Order.Infra.Data.Repositories;
using RssSE.WebApi.Core.User;
using RssSE.WebApi.Core.User.Interfaces;

namespace RssSE.Order.API.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //Commands
            services.AddScoped<IRequestHandler<CreateOrderCommand, ValidationResult>, OrderCommandHandler>();

            //Events
            services.AddScoped<INotificationHandler<FinishedOrderEvent>, OrderEventHandler>();

            //Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQueries, VoucherQueries>();
            services.AddScoped<IOrderQueries, OrderQueries>();

            //Data
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<OrdersDbContext>();
        }
    }
}
