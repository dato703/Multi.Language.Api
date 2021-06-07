using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multi.Language.Infrastructure.EventSourcing;

namespace Multi.Language.Infrastructure.Repositories
{
    public class EventStoreSqlRepository : IEventStoreRepository
    {
        private readonly UserContext _context;

        public EventStoreSqlRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<List<EventQueue>> All(Guid aggregateId)
        {
            return await _context.EventQueues.Where(x => x.AggregateId == aggregateId).ToListAsync();
        }

        public void Add(EventQueue @event)
        {
            _context.EventQueues.Add(@event);
        }

        public void Store(EventQueue @event)
        {
            _context.EventQueues.Add(@event);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}