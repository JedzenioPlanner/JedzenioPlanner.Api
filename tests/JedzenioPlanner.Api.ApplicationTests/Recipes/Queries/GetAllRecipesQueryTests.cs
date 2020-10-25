using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetAllRecipes;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Queries
{
    public class GetAllRecipesQueryTests
    {
        [Fact]
        public async void ShouldGetAllRecipesCorrectly()
        {
            var handler = new GetRecipesHandler(MockBuilder.BuildFakeRepository());
    
            var result = await handler.Handle(new GetRecipes(), CancellationToken.None);
    
            var expected = new[]
            {
                new Recipe
                {
                    Id = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                    Metadata = null,
                    Name = "sample-name",
                    Description = "sample-description",
                    PictureUrl = "https://example.com/sample-image.png",
                    Calories = 1234,
                    MealTypes = new[] {MealType.Snack},
                    Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                    Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
                },                
                new Recipe
                {
                    Id = new Guid("B8DBCACE-35A9-4D48-8A71-827E37D782A8"),
                    Metadata = null,
                    Name = "sample-name-2",
                    Description = "sample-description-2",
                    PictureUrl = "https://example.com/sample-image-2.png",
                    Calories = 4321,
                    MealTypes = new[] {MealType.Lunch},
                    Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                    Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
                },
            };
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}