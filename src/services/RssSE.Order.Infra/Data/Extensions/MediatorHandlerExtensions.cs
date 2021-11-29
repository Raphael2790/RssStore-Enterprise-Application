using Microsoft.EntityFrameworkCore;
using RssSE.Core.DomainObjects.BaseEntity;
using RssSE.Core.Mediator;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Order.Infra.Data.Extensions
{
    public static class MediatorHandlerExtensions
    {
        public static async Task PublishEvents<T>(this IMediatorHandler mediator, T context) where T : DbContext
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(x => x.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
