using FluentAssertions;
using JedzenioPlanner.Api.Application.Menus.Queries.FindRecipeQuery;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Menus.Queries.Validators
{
    public class FindRecipeTests
    {
        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var query = new FindRecipe(MealType.Breakfast, 600);
            var validator = new FindRecipeValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void DoesValidatorPreventRequestWithoutCorrectCaloriesTarget()
        {
            var query = new FindRecipe(MealType.Snack, -1);
            var validator = new FindRecipeValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
        }
    }
}