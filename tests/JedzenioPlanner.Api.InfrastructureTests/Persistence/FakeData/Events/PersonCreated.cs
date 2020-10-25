using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData.Events
{
    public class PersonCreated : Event<Person>
    {
        public string Name { get; set; }
        protected override Person Apply(Person entity)
        {
            entity.Name = Name;
            return entity;
        }
    }
}