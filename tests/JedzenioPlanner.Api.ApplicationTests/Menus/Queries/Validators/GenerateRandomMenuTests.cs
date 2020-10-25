using FluentAssertions;
using JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Menus.Queries.Validators
{
    public class GenerateRandomMenuTests
    {
        [Fact]
        public async void DoesValidatorAllowCorrectRequestWithoutMealPercentages()
        {
            var query = new GenerateRandomMenu(1000, 2);
            var validator = new GenerateRandomMenuValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void DoesValidatorAllowCorrectRequestWithMealPercentages()
        {
            var query = new GenerateRandomMenu(3250, 5, 20, 25, 35, 10);
            var validator = new GenerateRandomMenuValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void DoesValidatorPreventFromGeneratingMenuWithInvalidAmountOfMeals()
        {
            var query = new GenerateRandomMenu(1000, -1);
            var validator = new GenerateRandomMenuValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async void DoesValidatorPreventFromGeneratingMenuWithInvalidAmountOfCalories()
        {
            var query = new GenerateRandomMenu(-1, 4);
            var validator = new GenerateRandomMenuValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async void DoesValidatorPreventFromGeneratingMenuWithInvalidRatioBetweenMealsAmountAndCaloriesAmount()
        {
            var query = new GenerateRandomMenu(8000, 2);
            var validator = new GenerateRandomMenuValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async void DoesValidatorPreventFromGeneratingMenuWhenMealPercentageDoesNotSumpUpTo100()
        {
            var query = new GenerateRandomMenu(1000, 2, 20, 30, 30, 5);
            var validator = new GenerateRandomMenuValidator();

            var result = await validator.ValidateAsync(query);

            result.IsValid.Should().BeFalse();
        }
    }
}