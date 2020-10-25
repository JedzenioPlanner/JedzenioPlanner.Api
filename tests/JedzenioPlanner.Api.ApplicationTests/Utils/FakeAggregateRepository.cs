using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;

namespace JedzenioPlanner.Api.ApplicationTests.Utils
{
    public class FakeAggregateRepository<T> : IAggregatesRepository<T> where T : AggregateRoot
    {
        private readonly IEnumerable<T> _data;

        public FakeAggregateRepository(IEnumerable<T> data)
        {
            _data = data;
        }

        public Task<T> GetById(Guid id)
        {
            return Task.FromResult(_data.FirstOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult(_data);
        }
    }
}