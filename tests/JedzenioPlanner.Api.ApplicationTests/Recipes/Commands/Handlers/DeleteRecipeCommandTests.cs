using System;
using System.Threading;
using DeepEqual.Syntax;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Events;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Handlers
{
    public class DeleteRecipeCommandTests
    {
        [Fact]
        public async void ShouldDeleteRecipeCorrectly()
        {
            // "62CB0EE2-0CF7-4C72-9C51-80A90E8E420E" - sample recipe id to delete
            var command = new DeleteRecipe(new Guid("62CB0EE2-0CF7-4C72-9C51-80A90E8E420E"));
            var eventStore = new Mock<IEventStore<Recipe>>();
            var handler = new DeleteRecipeHandler(eventStore.Object, MockBuilder.BuildFakeRecipeEventsMapper(), MockBuilder.BuildFakeCurrentUserService(), MockBuilder.BuildFakeDateTimeService());
    
            await handler.Handle(command, CancellationToken.None);
    
            var expected = new RecipeRemoved
            {
                EntityId = new Guid("62CB0EE2-0CF7-4C72-9C51-80A90E8E420E"),
                Version = 0,
                Published = new DateTime(2010,1,1),
                AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB"
            };
            
            eventStore.Verify(x => x.AddEvent(It.Is<RecipeRemoved>(y => y.WithDeepEqual(expected).Compare())));
        }
    }
}