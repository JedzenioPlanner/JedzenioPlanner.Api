using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.MealPlan;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Moq;

namespace JedzenioPlanner.Api.ApplicationTests.Menus
{
    internal static class MockData
    {
        public static Recipe SampleBreakfast { get; } = new Recipe
        {
            Name = "Sample breakfast.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 650,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Breakfast}
        };

        public static Recipe SampleDinner { get; } = new Recipe
        {
            Name = "Sample dinner.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 1375,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Dinner}
        };

        public static Recipe SampleLunch { get; } = new Recipe
        {
            Name = "Sample lunch.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 725,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Lunch}
        };
    }

    internal static class MockBuilder
    {
        public static IMenuGenerator BuildFakeMenuGenerator()
        {
            var generatorMock = new Mock<IMenuGenerator>();

            generatorMock
                .Setup(x => x.FindRecipe(MealType.Breakfast, 650))
                .Returns(Task.FromResult(MockData.SampleBreakfast));

            var plan = new MealPlan(3, 2750, 0.275, 0.325, 0.40, 0);

            plan.MealPlanRows[0].Recipe = MockData.SampleBreakfast;
            plan.MealPlanRows[0].Calories = MockData.SampleBreakfast.Calories;
            plan.MealPlanRows[1].Recipe = MockData.SampleLunch;
            plan.MealPlanRows[1].Calories = MockData.SampleLunch.Calories;
            plan.MealPlanRows[2].Recipe = MockData.SampleDinner;
            plan.MealPlanRows[2].Calories = MockData.SampleDinner.Calories;

            generatorMock
                .Setup(x => x.GenerateMenu(It.IsAny<MealPlan>()))
                .Returns(Task.FromResult(plan));

            return generatorMock.Object;
        }
    }
}