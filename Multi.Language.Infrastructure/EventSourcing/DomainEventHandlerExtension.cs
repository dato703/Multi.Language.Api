using System.Linq;
using System.Threading.Tasks;
using App.Core;
using Microsoft.EntityFrameworkCore;

namespace Multi.Language.Infrastructure.EventSourcing
{
    public static class DomainEventHandlerExtension
    {
        public static async Task DispatchDomainEventsAsync<T>(this IDomainEventHandler domainEventHandler, T dbContext) where T : DbContext
        {
            var domainEntities = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await domainEventHandler.PublishEvent(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
