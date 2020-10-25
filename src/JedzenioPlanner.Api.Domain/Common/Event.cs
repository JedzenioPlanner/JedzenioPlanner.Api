using System;

namespace JedzenioPlanner.Api.Domain.Common
{
    public abstract class Event<T> where T : AggregateRoot
    {
        public Guid EntityId { get; set; }
        public DateTime Published { get; set; }
        public double Version { get; set; }
        public string AuthorId { get; set; }
        protected internal abstract T Apply(T entity);
    }
}