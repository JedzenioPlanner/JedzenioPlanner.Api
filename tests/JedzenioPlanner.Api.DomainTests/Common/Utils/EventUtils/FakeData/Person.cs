using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.DomainTests.Common.Utils.EventUtils.FakeData
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }
    }
}