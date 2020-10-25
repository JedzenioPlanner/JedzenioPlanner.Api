using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData.Events
{
    public class PersonRestored : Event<Person>
    {
        protected override Person Apply(Person entity)
        {
            entity.Metadata.Removed = false;
            return entity;
        }
    }
}