using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hangfire;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Infrastructure.Persistence.EventStore;
using JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData;
using JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData.Events;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.EventStore.EntityFrameworkEventStore
{
    public class AddEventTests
    {
        private readonly EventsDbContext _context;
        private readonly Mock<IBackgroundJobClient> _backgroundJobClient = new Mock<IBackgroundJobClient>();

        public AddEventTests()
        {
            var builder = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();
            _context = new EventsDbContext(builder.Options);
        }

        ~AddEventTests()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task Does_Add_Event_Works_When_Valid_Event()
        {
            var events = new Event<Person>[]
            {
                new PersonCreated
                {
                    EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                    Name = "John",
                    Published = new DateTime(2020,10,10,17,6,0),
                },
                new PersonUpdated
                {
                    EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                    Name = "Marcus",
                    Published = new DateTime(2020,10,10,17,6,1)
                },
                new PersonUpdated
                {
                    EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                    Name = "Mark",
                    Published = new DateTime(2020,10,10,17,6,2)
                },
            };
            var eventStore = new EntityFrameworkEventStore<Person>(_context, _backgroundJobClient.Object);
            var expected = new EventDatabaseEntity[]
            {
               new EventDatabaseEntity
               {
                   EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                   EntityVersion = 0,
                   EventDetails = "{\"Name\":\"John\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:00\",\"Version\":0.0,\"AuthorId\":null}",
                   EventName = "PersonCreated",
                   EventPublished = new DateTime(2020,10,10,17,6,0)
               },
               new EventDatabaseEntity
               {
                   EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                   EntityVersion = 1,
                   EventDetails = "{\"Name\":\"Marcus\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:01\",\"Version\":1.0,\"AuthorId\":null}",
                   EventName = "PersonUpdated",
                   EventPublished = new DateTime(2020,10,10,17,6,1)
               },
               new EventDatabaseEntity
               {
                   EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                   EntityVersion = 2,
                   EventDetails = "{\"Name\":\"Mark\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:02\",\"Version\":2.0,\"AuthorId\":null}",
                   EventName = "PersonUpdated",
                   EventPublished = new DateTime(2020,10,10,17,6,2)
               },
            }.AsEnumerable();

            foreach (var @event in events) await eventStore.AddEvent(@event);

            _context.Events.ToArray().Should().BeEquivalentTo(expected);
        }
    }
}