using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JedzenioPlanner.Api.Domain.Common.Persistence
{
    public interface IAggregatesRepository<T> where T : AggregateRoot
    {
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
    }
}