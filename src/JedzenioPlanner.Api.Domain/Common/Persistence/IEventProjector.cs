using System;
using System.Threading.Tasks;

namespace JedzenioPlanner.Api.Domain.Common.Persistence
{
    public interface IEventProjector
    {
        Task CatchUp();
        Task Notify(Guid entityId, double newVersion);
    }
    public interface IEventProjector<T> : IEventProjector where T: AggregateRoot
    {
    }
}