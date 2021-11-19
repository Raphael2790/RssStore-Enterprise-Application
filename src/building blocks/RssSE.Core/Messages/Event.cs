using FluentValidation.Results;
using MediatR;
using System;

namespace RssSE.Core.Messages
{
    public class Event : Message, INotification
    {
        protected DateTime TimeStamp { get; set; }
        protected ValidationResult ValidationResult;

        public Event()
        {
            TimeStamp = DateTime.Now;
        }

        public virtual bool IsValid() => throw new NotImplementedException();
    }
}
