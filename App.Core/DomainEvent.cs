using System;

namespace App.Core
{
    public abstract class DomainEvent
    {
        public Guid AggregateRootId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public Guid UserId { get; set; }

        protected DomainEvent()
        {
            EventDate = DateTimeOffset.Now;
        }

        protected DomainEvent(Guid aggregateRootId)
        {
            AggregateRootId = aggregateRootId;
            EventDate = DateTimeOffset.Now;
        }

    }
}
