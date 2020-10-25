using System;
using System.Threading;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Application.Recipes.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Events;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Handlers
{
    public class RestoreRecipeHandlerTests
    {
        private readonly Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();
        private readonly Mock<IDateTimeService> _dateTimeService = new Mock<IDateTimeService>();
        private readonly Mock<IEventStore<Recipe>> _eventStore = new Mock<IEventStore<Recipe>>();
        private readonly Mock<RecipeEventsMapper> _mapper = new Mock<RecipeEventsMapper>();
    
        [Fact]
        public async Task ShouldRestoreRecipeCorrectly()
        {
            var request = new RestoreRecipe
            {
                EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c")
            };
            var handler = new RestoreRecipeHandler(_eventStore.Object, MockBuilder.BuildFakeRecipeEventsMapper(), MockBuilder.BuildFakeDateTimeService(), MockBuilder.BuildFakeCurrentUserService());
            
            await handler.Handle(request, CancellationToken.None);
    
            var expected = new RecipeRestored
            {
                EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                Published = new DateTime(2010, 1, 1),
                AuthorId = "edb4a387-260e-43c1-aed8-eec4dbd0fc31"
            };
            _eventStore.Verify(x=>x.AddEvent(It.Is<RecipeRestored>(y => y.WithDeepEqual(expected).Compare())));
        }
    }
}