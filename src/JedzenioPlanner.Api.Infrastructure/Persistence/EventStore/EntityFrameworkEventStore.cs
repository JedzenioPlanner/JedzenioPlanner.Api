using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JedzenioPlanner.Api.Infrastructure.Persistence.EventStore
{
    public class EntityFrameworkEventStore<T> : IEventStore<T> where T : AggregateRoot, new()
    {
        private readonly EventsDbContext _context;
        private readonly IBackgroundJobClient _jobClient;

        public EntityFrameworkEventStore(EventsDbContext context, IBackgroundJobClient jobClient)
        {
            _context = context;
            _jobClient = jobClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event">Event's version is ignored.</param>
        /// <returns></returns>
        public async Task AddEvent(Event<T> @event)
        {
            var events = _context.Events
                .Where(x => x.EntityId == @event.EntityId)
                .Select(x => x.EntityVersion);
            if (await events.AnyAsync())
                @event.Version = await events.MaxAsync() + 1;
            else
                @event.Version = 0;
            var dto = new EventDatabaseEntity
            {
                EntityId = @event.EntityId,
                EntityVersion = @event.Version,
                EventPublished = @event.Published,
                EventName = @event.GetType().Name,
                EventDetails = JsonConvert.SerializeObject(@event)
            };
            await _context.AddAsync(dto);
            await _context.SaveChangesAsync();
            _jobClient.Enqueue<HangfirePersistenceJobs>(x => x.Notify(typeof(T), @event.EntityId, @event.Version));
        }
    }
}