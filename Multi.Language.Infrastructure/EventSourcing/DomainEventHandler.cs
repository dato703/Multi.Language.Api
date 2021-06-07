using System.Threading.Tasks;
using App.Core;
using MediatR;
using Newtonsoft.Json;

namespace Multi.Language.Infrastructure.EventSourcing
{
    public interface IDomainEventHandler
    {
        Task PublishEvent<T>(T @event) where T : DomainEvent;
    }
    public class DomainEventHandler : IDomainEventHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStoreRepository _eventStoreRepository;

        public DomainEventHandler(IMediator mediator, IEventStoreRepository eventStoreRepository)
        {
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }
        public async Task PublishEvent<T>(T @event) where T : DomainEvent
        {
            if (@event != null)
            {
                var serializedData = JsonConvert.SerializeObject(@event);

                var storedEvent = new EventQueue("", @event.AggregateRootId, @event.TransactionId,
                    @event.MessageType, @event.EventDate, @event.UserId, "Ip", serializedData);

                _eventStoreRepository.Add(storedEvent);

                await _mediator.Publish(@event);
            }
        }
    }
}
