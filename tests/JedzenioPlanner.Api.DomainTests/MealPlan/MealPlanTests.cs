using System;
using FluentAssertions;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Xunit;

namespace JedzenioPlanner.Api.DomainTests.MealPlan
{
    public class MealPlanTests
    {
        [Fact]
        public void CanInitializeMealPlan()
        {
            var snack = 0.1375 / (1 + 0.1375 * 2);
            var mealPlan = new Domain.MealPlan.MealPlan(5, 3250, 0.275 * (1 - snack * 2), 0.325 * (1 - snack * 2),
                0.4 * (1 - snack * 2), snack);

            mealPlan.MealPlanRows.Should().ContainSingle(x => x.Type == MealType.Breakfast && x.Calories == 701);
            mealPlan.MealPlanRows.Should().ContainSingle(x => x.Type == MealType.Lunch && x.Calories == 828);
            mealPlan.MealPlanRows.Should().ContainSingle(x => x.Type == MealType.Dinner && x.Calories == 1020);
            mealPlan.MealPlanRows.Should().Contain(x => x.Type == MealType.Snack && x.Calories == 350);
        }

        [Fact]
        public void CanRecalculate()
        {
            var snack = 0.1375 / (1 + 0.1375 * 2);
            var mealPlan = new Domain.MealPlan.MealPlan(5, 3000, 0.275 * (1 - snack * 2), 0.325 * (1 - snack * 2),
                0.4 * (1 - snack * 2), snack);

            mealPlan.MealPlanRows[0].Recipe = new Recipe
            {
                Name = "Sample breakfast.",
                Description = "Sample description.",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 650,
                Ingredients = new[] {"1. Sample ingredient"},
                Steps = new[] {"1. Sample step."},
                MealTypes = new[] {MealType.Breakfast}
            };

            mealPlan.Recalculate();

            mealPlan.MealPlanRows.Should()
                .ContainSingle(x => x.Type == MealType.Breakfast && Convert.ToInt32(x.Calories) == 650);
            mealPlan.MealPlanRows.Should()
                .ContainSingle(x => x.Type == MealType.Lunch && Convert.ToInt32(x.Calories) == 764);
            mealPlan.MealPlanRows.Should()
                .ContainSingle(x => x.Type == MealType.Dinner && Convert.ToInt32(x.Calories) == 940);
            mealPlan.MealPlanRows.Should().Contain(x => x.Type == MealType.Snack && Convert.ToInt32(x.Calories) == 324);
        }
    }
}