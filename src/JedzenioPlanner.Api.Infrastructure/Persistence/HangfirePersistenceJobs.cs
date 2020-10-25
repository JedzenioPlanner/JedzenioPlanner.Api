using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;

namespace JedzenioPlanner.Api.Infrastructure.Persistence
{
    public class HangfirePersistenceJobs
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly Type[] AggregateRootTypes;

        static HangfirePersistenceJobs()
        {
            AggregateRootTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass)
                .Where(x => x.BaseType == typeof(AggregateRoot))
                .ToArray();
        }

        public HangfirePersistenceJobs(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task CatchUp()
        {
            foreach (var aggregateRoot in AggregateRootTypes)
            {
                var type = typeof(IEventProjector<>).MakeGenericType(aggregateRoot);
                var result = _serviceProvider.GetService(type);
                switch (result)
                {
                    case IEventProjector eventProjector:
                        await eventProjector.CatchUp();
                        break;
                    case IEnumerable<IEventProjector> eventProjectors:
                    {
                        foreach (var projector in eventProjectors)
                        {
                            await projector.CatchUp();
                        }

                        break;
                    }
                }
            }
        }

        public async Task Notify(Type aggregateRootType, Guid entityId, double newVersion)
        {
            if (aggregateRootType.BaseType != typeof(AggregateRoot))
            {
                throw new Exception("aggregateRootType must have a base of AggregateRoot!");
            }
            var type = typeof(IEventProjector<>).MakeGenericType(aggregateRootType);
            var result = _serviceProvider.GetService(type);
            switch (result)
            {
                case IEventProjector eventProjector:
                    await eventProjector.Notify(entityId, newVersion);
                    break;
                case IEnumerable<IEventProjector> eventProjectors:
                {
                    foreach (var projector in eventProjectors)
                    {
                        await projector.Notify(entityId, newVersion);
                    }

                    break;
                }
            }
        }
    }
}