using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multi.Language.Infrastructure.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void Add(EventQueue @event);
        void Store(EventQueue @event);
        Task<List<EventQueue>> All(Guid aggregateId);
    }
}