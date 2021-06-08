using System;
using System.Linq;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Multi.Language.Infrastructure.Authorization;
using Newtonsoft.Json;

namespace Multi.Language.Infrastructure.EventSourcing
{
    public interface IDomainEventDispatcher
    {
        Task PublishEventAsync<T>(T @event) where T : DomainEvent;
        Task DispatchDomainEventsAsync<T>(T dbContext) where T : DbContext;
    }
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IAuthorizationService _authorizationService;

        public DomainEventDispatcher(IMediator mediator, IEventStoreRepository eventStoreRepository, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
            _authorizationService = authorizationService;
        }
        public async Task PublishEventAsync<T>(T @event) where T : DomainEvent
        {
            if (@event != null)
            {
                var serializedData = JsonConvert.SerializeObject(@event);

                //@event.UserId = _authorizationService.CurrentUserId;

                var storedEvent = new EventQueue("", @event.AggregateRootId, @event.TransactionId,
                    @event.MessageType, @event.EventDate, _authorizationService.CurrentUserId, _authorizationService.IpAddress, serializedData);

                _eventStoreRepository.Add(storedEvent);

                await _mediator.Publish(@event);
            }
        }

        public  async Task DispatchDomainEventsAsync<T>(T dbContext) where T : DbContext
        {
            var domainEntities = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());


            var transactionId = Guid.NewGuid();

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                if (domainEvent.TransactionId == Guid.Empty)
                {
                    domainEvent.TransactionId = transactionId;
                }
                await PublishEventAsync(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
