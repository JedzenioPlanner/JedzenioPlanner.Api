using System.Threading.Tasks;

namespace JedzenioPlanner.Api.Domain.Common.Persistence
{
    public interface IEventStore<T> where T : AggregateRoot
    {
        public Task AddEvent(Event<T> @event);
    }
}