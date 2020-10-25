using System;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Infrastructure.Persistence.EventStore;
using JedzenioPlanner.Api.Infrastructure.Persistence.Projector;
using JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.Projector.EntityFrameworkEventsProjector
{
    public class CatchUpTests
    {
        private readonly EventsDbContext _context;

        public CatchUpTests()
        {
            var builder = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();
            _context = new EventsDbContext(builder.Options);
        }

        ~CatchUpTests()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task Does_Catch_Up_To_New_Object_Work()
        {
            var target = new Mock<IEventProjectorTarget<Person>>();
            var projector = new EntityFrameworkEventsProjector<Person>(_context, new []{target.Object});
            var expected = new Person
            {
                Id = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                Name = "Mark",
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        Published = new DateTime(2020, 10, 10, 17, 06, 0)
                    },
                    Updates = new[]
                    {
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 1),
                            Version = 1
                        },
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 2),
                            Version = 2
                        }
                    },
                    Removed = false,
                    Version = 2
                }
            };
            await _context.Events.AddRangeAsync(new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 0,
                EventDetails = "{\"Name\":\"John\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:00\",\"Version\":0.0,\"AuthorId\":null}",
                EventName = "PersonCreated",
                EventPublished = new DateTime(2020,10,10,17,6,0)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 1,
                EventDetails = "{\"Name\":\"Marcus\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:01\",\"Version\":1.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,1)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 2,
                EventDetails = "{\"Name\":\"Mark\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:02\",\"Version\":2.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,2)
            });
            await _context.SaveChangesAsync();

            await projector.CatchUp();
            
            target.Verify(x=>x.Add(It.Is<Person>(y=>y.IsDeepEqual(expected))));
            target.Verify(x=>x.SaveChanges());
        }

        [Fact]
        public async Task Does_Catch_Up_To_New_Version_Work()
        {
            var target = new Mock<IEventProjectorTarget<Person>>();
            var projector = new EntityFrameworkEventsProjector<Person>(_context, new []{target.Object});
            var actual = new Person
            {
                Id = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                Name = "Mark",
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        Published = new DateTime(2020, 10, 10, 17, 06, 0)
                    },
                    Updates = new[]
                    {
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 1),
                            Version = 1
                        },
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 2),
                            Version = 2
                        }
                    },
                    Removed = false,
                    Version = 2
                }
            };
            var expected = new Person
            {
                Id = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                Name = "John",
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        Published = new DateTime(2020, 10, 10, 17, 06, 0)
                    },
                    Updates = new[]
                    {
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 1),
                            Version = 1
                        },
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 2),
                            Version = 2
                        },
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 3),
                            Version = 3
                        },
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 4),
                            Version = 4
                        }
                    },
                    Removed = false,
                    Version = 4
                }
            };
            await _context.Events.AddRangeAsync(new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 3,
                EventDetails = "{\"Name\":\"Marcus\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:03\",\"Version\":3.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,3)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 4,
                EventDetails = "{\"Name\":\"John\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:04\",\"Version\":4.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,4)
            });
            await _context.SaveChangesAsync();
            target
                .Setup(x => x.GetById(It.Is<Guid>(y => y == Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"))))
                .ReturnsAsync(actual);

            await projector.CatchUp();
            
            target.Verify(x=>x.Update(It.Is<Person>(y=>y.IsDeepEqual(expected))));
            target.Verify(x=>x.SaveChanges());
        }

        [Fact]
        public async Task Does_Not_Add_Removed_Entity()
        {
            var target = new Mock<IEventProjectorTarget<Person>>();
            var projector = new EntityFrameworkEventsProjector<Person>(_context, new []{target.Object});
            await _context.Events.AddRangeAsync(new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 0,
                EventDetails = "{\"Name\":\"John\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:00\",\"Version\":0.0,\"AuthorId\":null}",
                EventName = "PersonCreated",
                EventPublished = new DateTime(2020,10,10,17,6,0)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 1,
                EventDetails = "{\"Name\":\"Marcus\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:01\",\"Version\":1.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,1)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 2,
                EventDetails = "{\"Name\":\"Mark\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:02\",\"Version\":2.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,2)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 3,
                EventDetails = "{\"EntityId\":\"82414384-6144-45fb-a4b0-860910e2f95b\",\"Published\":\"2020-10-10T17:06:03\",\"Version\":3.0,\"AuthorId\":null}",
                EventName = "PersonRemoved",
                EventPublished = new DateTime(2020,10,10,17,6,3)
            });
            await _context.SaveChangesAsync();

            await projector.CatchUp();
            
            target.Verify(x=>x.Add(It.IsAny<Person>()), Times.Never);
            target.Verify(x=>x.SaveChanges(), Times.Never);
        }
        
        [Fact]
        public async Task Does_Remove_Removed_Entity()
        {
            var target = new Mock<IEventProjectorTarget<Person>>();
            var projector = new EntityFrameworkEventsProjector<Person>(_context, new []{target.Object});
            var actual = new Person
            {
                Id = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                Name = "Mark",
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        Published = new DateTime(2020, 10, 10, 17, 06, 0)
                    },
                    Updates = new[]
                    {
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 1),
                            Version = 1
                        },
                        new Update
                        {
                            Published = new DateTime(2020, 10, 10, 17, 06, 2),
                            Version = 2
                        }
                    },
                    Removed = false,
                    Version = 2
                }
            };
            await _context.Events.AddRangeAsync(new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 3,
                EventDetails = "{\"Name\":\"Marcus\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:03\",\"Version\":3.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,3)
            }, new EventDatabaseEntity
            {
                EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                EntityVersion = 4,
                EventDetails = "{\"Name\":\"John\",\"EntityId\":\"fa710953-2128-47c6-bef5-391ce66d0404\",\"Published\":\"2020-10-10T17:06:04\",\"Version\":4.0,\"AuthorId\":null}",
                EventName = "PersonUpdated",
                EventPublished = new DateTime(2020,10,10,17,6,4)
            },
                new EventDatabaseEntity
                {
                    EntityId = Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"),
                    EntityVersion = 5,
                    EventDetails = "{\"EntityId\":\"82414384-6144-45fb-a4b0-860910e2f95b\",\"Published\":\"2020-10-10T17:06:05\",\"Version\":5.0,\"AuthorId\":null}",
                    EventName = "PersonRemoved",
                    EventPublished = new DateTime(2020,10,10,17,6,5)
                });
            await _context.SaveChangesAsync();
            target
                .Setup(x => x.GetById(It.Is<Guid>(y => y == Guid.Parse("fa710953-2128-47c6-bef5-391ce66d0404"))))
                .ReturnsAsync(actual);

            await projector.CatchUp();
            
            target.Verify(x=>x.Delete(It.Is<Person>(y=>y.IsDeepEqual(actual))));
            target.Verify(x=>x.SaveChanges());
        }
    }
}