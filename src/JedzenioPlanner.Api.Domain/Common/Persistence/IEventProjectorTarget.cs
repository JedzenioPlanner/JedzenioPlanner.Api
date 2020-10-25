using System.Threading.Tasks;

namespace JedzenioPlanner.Api.Domain.Common.Persistence
{
    public interface IEventProjectorTarget<T> : IAggregatesRepository<T> where T: AggregateRoot
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task SaveChanges();
    }
}