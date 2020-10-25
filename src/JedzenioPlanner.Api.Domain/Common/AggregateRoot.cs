using System;
using JedzenioPlanner.Api.Domain.Common.SubElements;

namespace JedzenioPlanner.Api.Domain.Common
{
    public abstract class AggregateRoot
    {
        public AggregateRoot()
        {
            Metadata = new Metadata();
        }
        public Guid Id { get; set; }
        public Metadata Metadata { get; set; }
    }
}