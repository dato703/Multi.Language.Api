using Newtonsoft.Json;
using System.Collections.Generic;

namespace App.Core
{
    public abstract class AggregateRoot<T> : Entity<T> where T : struct
    {
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        [JsonIgnore]
        public virtual IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

        public virtual void HandleEvent(DomainEvent newEvent)
        {
            _domainEvents.Add(newEvent);
        }

        public virtual void ClearEvents()
        {
            _domainEvents.Clear();
        }

    }
}
