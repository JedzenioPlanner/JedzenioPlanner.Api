using System;
using System.Linq;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Infrastructure.Persistence;
using JedzenioPlanner.Api.InfrastructureTests.Persistence.FakeData;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.Hangfire
{
    internal class HangfirePersistenceJobsBuilder
    {
        public HangfirePersistenceJobsBuilder()
        {
            EventProjectorMock = new Mock<IEventProjector>();
            
            var serviceProviderMock = new Mock<IServiceProvider>();
            
            serviceProviderMock
                .Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(EventProjectorMock.Object);
            
            Jobs = new HangfirePersistenceJobs(serviceProviderMock.Object);
        }
        
        public Mock<IEventProjector> EventProjectorMock { get; set; }
        public  HangfirePersistenceJobs Jobs { get; set; }
    }
    
    public class HangfirePersistenceJobsTests
    {
        [Fact]
        public async void ShouldCatchUpJobWorkCorrectly()
        {
            var aggregateRootTypesAmount = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass)
                .Count(x => x.BaseType == typeof(AggregateRoot));
            var builder = new HangfirePersistenceJobsBuilder();
            
            await builder.Jobs.CatchUp();
            
            builder.EventProjectorMock.Verify(x => x.CatchUp(), Times.Exactly(aggregateRootTypesAmount));
            builder.EventProjectorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldNotifyJobWorkCorrectly()
        {
            var type = typeof(Person);
            var builder = new HangfirePersistenceJobsBuilder();
            
            await builder.Jobs.Notify(type, new Guid("AC941F5E-91C8-4200-8C42-92CB89696517"), 2);

            builder.EventProjectorMock.Verify(x => x.Notify(new Guid("AC941F5E-91C8-4200-8C42-92CB89696517"), 2));
            builder.EventProjectorMock.VerifyNoOtherCalls();
        }
    }
}