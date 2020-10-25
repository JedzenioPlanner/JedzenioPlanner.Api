using JedzenioPlanner.Api.Domain.MealPlan;
using MediatR;

namespace JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery
{
    public class GenerateRandomMenu : IRequest<MealPlan>
    {
        public GenerateRandomMenu(int caloriesTarget, int mealsAmount,
            int breakfastPercentage = 0, int lunchPercentage = 0, int dinnerPercentage = 0, int snackPercentage = 0)
        {
            CaloriesTarget = caloriesTarget;
            MealsAmount = mealsAmount;

            BreakfastPercentage = breakfastPercentage;
            LunchPercentage = lunchPercentage;
            DinnerPercentage = dinnerPercentage;
            SnackPercentage = snackPercentage;
        }

        public int CaloriesTarget { get; set; }
        public int MealsAmount { get; set; }

        public int BreakfastPercentage { get; set; }
        public int LunchPercentage { get; set; }
        public int DinnerPercentage { get; set; }
        public int SnackPercentage { get; set; }
    }
}