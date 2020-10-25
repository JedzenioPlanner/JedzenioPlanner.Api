using System.Threading;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Menus.Queries.FindRecipeQuery;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Menus.Queries.Handlers
{
    public class FindRecipeTests
    {
        [Fact]
        public async void ShouldFindSingleRecipeCorrectly()
        {
            var query = new FindRecipe(MealType.Breakfast, 650);
            var handler = new FindRecipeHandler(MockBuilder.BuildFakeMenuGenerator());

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(MockData.SampleBreakfast);
        }
    }
}