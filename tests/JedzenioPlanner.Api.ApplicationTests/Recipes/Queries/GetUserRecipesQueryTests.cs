using System;
using System.Threading;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetUserRecipes;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Queries
{
    public class GetUserRecipesQueryTests
    {
        [Fact]
        public async void ShouldReturnUserRecipesCorrectly()
        {
            var query = new GetUserRecipes("9E09950B-47DE-4BAB-AA79-C29414312ECB");
            var handler = new GetUserRecipesHandler(MockBuilder.BuildFakeRepository());

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(2)
                .And.ContainSingle(x => x.Id == new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"))
                .And.ContainSingle(x => x.Id == new Guid("B8DBCACE-35A9-4D48-8A71-827E37D782A8"));
        }
    }
}