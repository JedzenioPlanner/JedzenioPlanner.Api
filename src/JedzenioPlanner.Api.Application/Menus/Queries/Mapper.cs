using System;
using JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery;
using JedzenioPlanner.Api.Domain.MealPlan;

namespace JedzenioPlanner.Api.Application.Menus.Queries
{
    public static class Mapper
    {
        public static MealPlan Map(GenerateRandomMenu request)
        {
            if (request.BreakfastPercentage != 0 || request.LunchPercentage != 0 || request.DinnerPercentage != 0 ||
                request.SnackPercentage != 0)
                return new MealPlan(request.MealsAmount, request.CaloriesTarget,
                    Convert.ToDouble(request.BreakfastPercentage) / 100,
                    Convert.ToDouble(request.LunchPercentage) / 100,
                    Convert.ToDouble(request.DinnerPercentage) / 100,
                    Convert.ToDouble(request.SnackPercentage) / 100);

            var snack = 0.1375 / (1 + 0.1375 * (request.MealsAmount - 3));
            return request.MealsAmount switch
            {
                2 => new MealPlan(request.MealsAmount, request.CaloriesTarget, 0.4, 0, 0.6, 0),
                3 => new MealPlan(request.MealsAmount, request.CaloriesTarget, 0.275, 0.325, 0.40, 0),
                _ => new MealPlan(request.MealsAmount, request.CaloriesTarget,
                    0.275 * (1 - snack * (request.MealsAmount - 3)),
                    0.325 * (1 - snack * (request.MealsAmount - 3)),
                    0.4 * (1 - snack * (request.MealsAmount - 3)),
                    snack)
            };
        }
    }
}