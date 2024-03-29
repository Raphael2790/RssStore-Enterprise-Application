﻿using MediatR;
using RssSE.Client.API.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssSE.Client.API.Application.Handlers
{
    public class CustomerEventHandler : INotificationHandler<RegisteredCustomerEvent>
    {
        public Task Handle(RegisteredCustomerEvent message, CancellationToken cancellationToken)
        {
            //Oportunidade para enviar um email de boas vindas
            return Task.CompletedTask;
        }
    }
}
