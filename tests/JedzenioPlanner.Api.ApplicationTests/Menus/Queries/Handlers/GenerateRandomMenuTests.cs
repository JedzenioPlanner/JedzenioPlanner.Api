using System.Threading;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery;
using JedzenioPlanner.Api.Domain.MealPlan;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Menus.Queries.Handlers
{
    public class GenerateRandomMenuTests
    {
        [Fact]
        public async void ShouldGenerateRandomMenuCorrectly()
        {
            var query = new GenerateRandomMenu(2750, 3);
            var handler = new GenerateRandomMenuHandler(MockBuilder.BuildFakeMenuGenerator());

            var result = await handler.Handle(query, CancellationToken.None);

            var plan = new MealPlan(3, 2750, 0.275, 0.325, 0.40, 0);

            plan.MealPlanRows[0].Recipe = MockData.SampleBreakfast;
            plan.MealPlanRows[0].Calories = MockData.SampleBreakfast.Calories;
            plan.MealPlanRows[1].Recipe = MockData.SampleLunch;
            plan.MealPlanRows[1].Calories = MockData.SampleLunch.Calories;
            plan.MealPlanRows[2].Recipe = MockData.SampleDinner;
            plan.MealPlanRows[2].Calories = MockData.SampleDinner.Calories;

            result.Should().BeEquivalentTo(plan);
        }
    }
}