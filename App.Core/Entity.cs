using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Core
{
    public class Entity
    {
        private List<DomainEvent> _domainEvents;
        [JsonIgnore]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(DomainEvent eventItem)
        {
            _domainEvents ??= new List<DomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
    public class Entity<T> : Entity
    {
        public T Id { get; set; }

        public Entity()
        {

        }
    }
}
