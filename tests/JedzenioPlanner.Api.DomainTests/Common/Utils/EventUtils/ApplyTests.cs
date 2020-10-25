using System;
using System.Linq;
using FluentAssertions;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Common.Exceptions;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Common.Utils;
using JedzenioPlanner.Api.DomainTests.Common.Utils.EventUtils.FakeData;
using JedzenioPlanner.Api.DomainTests.Common.Utils.EventUtils.FakeData.Events;
using Xunit;

namespace JedzenioPlanner.Api.DomainTests.Common.Utils.EventUtils
{
    public class ApplyTests
    {
        [Fact]
        public void Does_Apply_Merges_Correct_Events()
        {
            var events = new Event<Person>[]
            {
                new PersonCreated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,0),
                    Name = "John"
                },
                new PersonUpdated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,1),
                    Name = "Marcus",
                    Version = 1
                },
                new PersonUpdated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,2),
                    Name = "Mark",
                    Version = 2
                },
            };
            var expected = new Person
            {
                Id = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                Name = "Mark",
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        Published = new DateTime(2020,10,10,16,0,0)
                    },
                    Version = 2,
                    Updates = new []
                    {
                        new Update
                        {
                            Version = 1,
                            Published = new DateTime(2020,10,10,16,0,1)
                        }, 
                        new Update
                        {
                            Version = 2,
                            Published = new DateTime(2020,10,10,16,0,2)
                        }
                    }
                }
            };

            var result = events.Aggregate(new Person(), (current, @event) => @event.Apply(current));
            
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Does_Apply_Throws_Incompatible_Version_Exception_When_Two_Same_Versions()
        {
            var events = new Event<Person>[]
            {
                new PersonCreated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,0),
                    Name = "John"
                },
                new PersonUpdated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,1),
                    Name = "Marcus",
                    Version = 1
                },
                new PersonUpdated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,1),
                    Name = "Mark",
                    Version = 1
                },
            };

            Action action = () => events.Aggregate(new Person(), (current, @event) => @event.Apply(current));

            action.Should().ThrowExactly<IncompatibleVersionException>();
        }

        [Fact]
        public void Does_Apply_Merges_Correct_Event()
        {
            var events = new Event<Person>[]
            {
                new PersonCreated
                {
                    EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                    Published = new DateTime(2020,10,10,16,0,0),
                    Name = "John"
                }
            };
            var expected = new Person
            {
                Id = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                Name = "John",
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        Published = new DateTime(2020,10,10,16,0,0)
                    },
                    Version = 0,
                    Updates = ArraySegment<Update>.Empty
                }
            };

            var result = events.Aggregate(new Person(), (current, @event) => @event.Apply(current));
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}