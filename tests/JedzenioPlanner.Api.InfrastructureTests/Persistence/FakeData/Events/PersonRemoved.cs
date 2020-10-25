using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData.Events
{
    public class PersonRemoved : Event<Person>
    {
        protected override Person Apply(Person entity)
        {
            entity.Metadata.Removed = true;
            return entity;
        }
    }
}