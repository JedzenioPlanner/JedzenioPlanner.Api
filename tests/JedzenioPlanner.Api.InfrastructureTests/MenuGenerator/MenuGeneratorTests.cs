using FluentAssertions;
using JedzenioPlanner.Api.Domain.MealPlan;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Xunit;

namespace JedzenioPlanner.Api.InfrastructureTests.MenuGenerator
{
    public class MenuGeneratorTests
    {
        [Fact]
        public async void ShouldFindSingleRecipeCorrectly()
        {
            var generator = new Infrastructure.MenuGenerator.MenuGenerator(new FakeRecipesDbContext());

            var result = await generator.FindRecipe(MealType.Breakfast, 701);

            var expected = MockData.SampleBreakfast;

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void ShouldGenerateMenuCorrectly()
        {
            var generator = new Infrastructure.MenuGenerator.MenuGenerator(new FakeRecipesDbContext());
            var snack = 0.1375 / (1 + 0.1375 * 2);

            var result = await generator.GenerateMenu(new MealPlan(5, 3250, 0.275 * (1 - snack * 2),
                0.325 * (1 - snack * 2), 0.4 * (1 - snack * 2), snack));

            var plan = new MealPlan(5, 3250, 0.275 * (1 - snack * 2), 0.325 * (1 - snack * 2), 0.4 * (1 - snack * 2),
                snack);

            plan.MealPlanRows[0].Recipe = MockData.SampleBreakfast;
            plan.MealPlanRows[0].Calories = MockData.SampleBreakfast.Calories;
            plan.MealPlanRows[1].Recipe = MockData.SampleLunch;
            plan.MealPlanRows[1].Calories = MockData.SampleLunch.Calories;
            plan.MealPlanRows[2].Recipe = MockData.SampleDinner;
            plan.MealPlanRows[2].Calories = MockData.SampleDinner.Calories;
            plan.MealPlanRows[3].Recipe = MockData.SampleSnack;
            plan.MealPlanRows[3].Calories = MockData.SampleSnack.Calories;
            plan.MealPlanRows[4].Recipe = MockData.SampleSnack;
            plan.MealPlanRows[4].Calories = MockData.SampleSnack.Calories;

            result.Should().BeEquivalentTo(plan);
        }
    }
}