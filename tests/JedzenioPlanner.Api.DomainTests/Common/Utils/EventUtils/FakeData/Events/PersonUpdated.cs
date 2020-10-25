using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.DomainTests.Common.Utils.EventUtils.FakeData.Events
{
    public class PersonUpdated : Event<Person>
    {
        public string Name { get; set; }
        protected override Person Apply(Person entity)
        {
            entity.Name = Name ?? entity.Name;
            return entity;
        }
    }
}