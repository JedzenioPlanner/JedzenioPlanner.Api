using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }
    }
}