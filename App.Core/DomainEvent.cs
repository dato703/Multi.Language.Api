using System;
using MediatR;

namespace App.Core
{
    public abstract class DomainEvent : INotification
    {
        public Guid AggregateRootId { get; protected set; }
        public Guid TransactionId { get; protected set; }
        public DateTimeOffset EventDate { get; protected set; }
        public Guid UserId { get; protected set; }
        public string MessageType { get; protected set; }

        protected DomainEvent()
        {
            EventDate = DateTimeOffset.Now;
            MessageType = this.GetType().Name;
        }

        protected DomainEvent(Guid aggregateRootId)
        {
            AggregateRootId = aggregateRootId;
            EventDate = DateTimeOffset.Now;
            MessageType = this.GetType().Name;
        }

    }
}
