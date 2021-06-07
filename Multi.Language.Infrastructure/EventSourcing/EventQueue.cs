using System;
using App.Core;

namespace Multi.Language.Infrastructure.EventSourcing
{
    public class EventQueue : Entity<long>
    {
        public EventQueue()
        {
            
        }
        public EventQueue(string stream, Guid aggregateId, Guid transactionId, string eventName, DateTimeOffset eventDate, Guid? userId, string ipAddress, string data)
        {
            MessageType = this.GetType().Name;
            Stream = stream;
            AggregateId = aggregateId;
            TransactionId = transactionId;
            EventName = eventName;
            EventDate = eventDate;
            UserId = userId;
            IpAddress = ipAddress;
            Data = data;
        }

        public string MessageType { get; private set; }
        public string Stream { get; private set; }
        public Guid AggregateId { get; private set; }
        public Guid TransactionId { get; private set; }
        public string EventName { get; private set; }
        public DateTimeOffset EventDate { get; private set; }
        public Guid? UserId { get; private set; }
        public string IpAddress { get; private set; }
        public string Data { get; private set; }

    }
}
