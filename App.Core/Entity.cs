using System.Collections.Generic;
using MediatR;
using Newtonsoft.Json;

namespace App.Core
{
    public class Entity
    {
        private List<INotification> _domainEvents;
        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
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
