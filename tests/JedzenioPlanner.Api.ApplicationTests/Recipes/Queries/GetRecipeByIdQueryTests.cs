using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Queries
{
    public class GetRecipeByIdQueryTests
    {
        [Fact]
        public async void ShouldGetRecipeByIdCorrectly()
        {
            // "447EA0EF-F828-486A-91A9-0EDBC01D0B89" - sample existing recipe
            var query = new GetRecipeById(new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"), false);
            var handler = new GetRecipeByIdHandler(MockBuilder.BuildFakeRepository());
            
            var actual = await handler.Handle(query, CancellationToken.None);
        
            var expected = new Recipe
            {
                Id = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Metadata = new Metadata
                {
                    Version = 0,
                    Removed = false,
                    Creation = new Creation {AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB", Published = new DateTime(2010, 1, 1) },
                    Updates = new Update[0]
                },
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async void ShouldGetRecipeByIdWithoutMetadataCorrectly()
        {
            // "447EA0EF-F828-486A-91A9-0EDBC01D0B89" - sample existing recipe
            var query = new GetRecipeById(new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"));
            var handler = new GetRecipeByIdHandler(MockBuilder.BuildFakeRepository());

            var actual = await handler.Handle(query, CancellationToken.None);

            var expected = new Recipe
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
            };

            actual.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            // "447EA0EF-F828-486A-91A9-0EDBC01D0B89" - sample existing recipe
            var query = new GetRecipeById(new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"));
            var validator = new GetRecipeByIdValidator(MockBuilder.BuildFakeRepository());
        
            var result = await validator.ValidateAsync(query);
        
            result.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public async void DoesValidatorPreventRequestWithNonExistentRecipe()
        {
            // "D8B765B2-D11B-46E8-8142-E7A9BFBA7286" - sample non existent recipe
            var query = new GetRecipeById(new Guid("D8B765B2-D11B-46E8-8142-E7A9BFBA7286"));
            var validator = new GetRecipeByIdValidator(MockBuilder.BuildFakeRepository());
            
            var result = await validator.ValidateAsync(query);
        
            result.IsValid.Should().BeFalse();
        }
    }
}