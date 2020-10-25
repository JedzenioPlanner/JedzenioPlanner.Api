using System;
using System.Threading;
using DeepEqual.Syntax;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Domain.Recipes.Events;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Handlers
{
    public class CreateRecipeCommandTests
    {
        [Fact]
        public async void ShouldCreateRecipeCorrectly()
        {
            var command = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var eventStore = new Mock<IEventStore<Recipe>>();            
            var handler = new CreateRecipeHandler(eventStore.Object, MockBuilder.BuildFakeRecipeEventsMapper(), MockBuilder.BuildFakeCurrentUserService(), MockBuilder.BuildFakeDateTimeService());
    
            await handler.Handle(command, CancellationToken.None);
    
            var expected = new RecipeCreated
            {
                EntityId = new Guid("7ADC9EF0-6A2A-4DE5-9C27-D4C2A4D9D5B6"),
                Published = new DateTime(2010,1,1),
                Version = 0,
                AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB",
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
    
            eventStore.Verify(x => x.AddEvent(It.Is<RecipeCreated>(y 
                => y.WithDeepEqual(expected).IgnoreSourceProperty(z => z.EntityId).Compare()
            )));
        }
    }
}