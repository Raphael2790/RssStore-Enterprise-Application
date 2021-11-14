using RssSE.Core.DomainObjects.Interfaces;
using System;

namespace RssSE.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
