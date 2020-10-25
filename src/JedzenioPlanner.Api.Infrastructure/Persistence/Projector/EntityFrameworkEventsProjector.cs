using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Common.Utils;
using JedzenioPlanner.Api.Infrastructure.Persistence.EventStore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JedzenioPlanner.Api.Infrastructure.Persistence.Projector
{
    public class EntityFrameworkEventsProjector<T> : IEventProjector<T> where T : AggregateRoot, new()
    {
        private readonly EventsDbContext _context;
        private readonly IEnumerable<IEventProjectorTarget<T>> _targets;

        public EntityFrameworkEventsProjector(EventsDbContext context, IEnumerable<IEventProjectorTarget<T>> targets)
        {
            _context = context;
            _targets = targets;
        }

        public async Task CatchUp()
        {
            var ids = await _context.Events
                .Select(x => x.EntityId).Distinct().ToArrayAsync();
            foreach (var target in _targets)
            {
                var anyChanges = false;
                foreach (var id in ids)
                {
                    var databaseEntity = await target.GetById(id);
                    if (databaseEntity == null)
                    {
                        var events = _context.Events
                            .Where(x => x.EntityId == id)
                            .OrderBy(x => x.EntityVersion);
                        var entity = new T();
                        entity = Enumerable.Aggregate(events, entity,
                            (current, @event) => GetEvent(@event).Apply(current));
                        if (!entity.Metadata.Removed)
                        {
                            anyChanges = true;
                            await target.Add(entity);
                        }
                    }
                    else
                    {
                        var maxVersion = await _context.Events
                            .Where(x => x.EntityId == id)
                            .Select(x => x.EntityVersion)
                            .MaxAsync();
                        if (databaseEntity.Metadata.Version < maxVersion)
                        {
                            var events = _context.Events
                                .Where(x => x.EntityId == id)
                                .Where(x => x.EntityVersion > databaseEntity.Metadata.Version)
                                .OrderBy(x => x.EntityVersion);
                            var entity = Enumerable.Aggregate(events, databaseEntity,
                                (current, @event) => GetEvent(@event).Apply(current));
                            if (entity.Metadata.Removed)
                            {
                                await target.Delete(databaseEntity);
                            }
                            else
                            {
                                await target.Update(entity);
                            }
                            anyChanges = true;
                        }
                    }
                }

                if (anyChanges) await target.SaveChanges();
            }
        }

        public async Task Notify(Guid entityId, double newVersion)
        {
            foreach (var target in _targets)
            {
                var databaseEntity = await target.GetById(entityId);
                if (databaseEntity == null)
                {
                    var events = _context.Events
                        .Where(x => x.EntityId == entityId)
                        .Where(x => x.EntityVersion <= newVersion)
                        .AsAsyncEnumerable();
                    var entity = new T();
                    await foreach (var @event in events) entity = GetEvent(@event).Apply(entity);
                    if (!entity.Metadata.Removed)
                    {
                        await target.Add(entity);
                        await target.SaveChanges();
                    }
                }
                else if (databaseEntity.Metadata.Version < newVersion)
                {
                    var events = _context.Events
                        .Where(x => x.EntityId == entityId)
                        .Where(x => x.EntityVersion > databaseEntity.Metadata.Version && x.EntityVersion <= newVersion)
                        .AsAsyncEnumerable();
                    var entity = databaseEntity;
                    await foreach (var @event in events) entity = GetEvent(@event).Apply(entity);
                    if (entity.Metadata.Removed)
                    {
                        await target.Delete(databaseEntity);
                    }
                    else
                    {
                        await target.Update(entity);
                    }
                    await target.SaveChanges();
                }
            }
        }


        private static Event<T> GetEvent(EventDatabaseEntity @event)
        {
            return JsonConvert.DeserializeObject(@event.EventDetails, EventUtils.GetEvent<T>(@event.EventName))
                as Event<T>;
        }
    }
}