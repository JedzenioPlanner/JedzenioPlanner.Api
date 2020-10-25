using System;
using System.Threading;
using DeepEqual.Syntax;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Domain.Recipes.Events;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Handlers
{
    public class UpdateRecipeCommandTests
    {
        [Fact]
        public async void ShouldUpdateRecipeCorrectly()
        {
            // "051F13A0-4796-4B1C-9797-EC99F08CF25E" - sample recipe id to update
            var command = new UpdateRecipe
            {
                EntityId = new Guid("051F13A0-4796-4B1C-9797-EC99F08CF25E"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var eventStore = new Mock<IEventStore<Recipe>>();
            var handler = new UpdateRecipeHandler(eventStore.Object, MockBuilder.BuildFakeRecipeEventsMapper(), MockBuilder.BuildFakeCurrentUserService(), MockBuilder.BuildFakeDateTimeService());
    
            await handler.Handle(command, CancellationToken.None);
    
            var expected = new RecipeUpdated
            {
                EntityId = new Guid("051F13A0-4796-4B1C-9797-EC99F08CF25E"), 
                Published = new DateTime(2010,1,1),
                Version = 0,
                AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB",
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            eventStore.Verify(x => x.AddEvent(It.Is<RecipeUpdated>(y => y.WithDeepEqual(expected).Compare())));
        }
    }
}