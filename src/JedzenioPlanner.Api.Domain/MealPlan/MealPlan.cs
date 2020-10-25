using System;
using System.Collections.Generic;
using System.Linq;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Domain.MealPlan
{
    public class MealPlan
    {
        public MealPlan(int mealsAmount, int totalCalories, double breakfastPercentage, double lunchPercentage,
            double dinnerPercentage, double snackPercentage)
        {
            MealsAmount = mealsAmount;
            TotalCalories = totalCalories;
            MealPlanRows = SetupMealPlanRowsWithGivenValues(
                breakfastPercentage, lunchPercentage, dinnerPercentage, snackPercentage
            );
        }

        public int MealsAmount { get; set; }
        public int TotalCalories { get; set; }
        public MealPlanRow[] MealPlanRows { get; set; }

        private MealPlanRow[] SetupMealPlanRowsWithGivenValues(double breakfastPercentage, double lunchPercentage,
            double dinnerPercentage, double snackPercentage)
        {
            return MealsAmount switch
            {
                2 => new[]
                {
                    new MealPlanRow(MealType.Breakfast, breakfastPercentage * TotalCalories),
                    new MealPlanRow(MealType.Dinner, dinnerPercentage * TotalCalories)
                },
                3 => new[]
                {
                    new MealPlanRow(MealType.Breakfast, breakfastPercentage * TotalCalories),
                    new MealPlanRow(MealType.Lunch, lunchPercentage * TotalCalories),
                    new MealPlanRow(MealType.Dinner, dinnerPercentage * TotalCalories)
                },
                _ => new[]
                {
                    new MealPlanRow(MealType.Breakfast, breakfastPercentage * TotalCalories),
                    new MealPlanRow(MealType.Lunch, lunchPercentage * TotalCalories),
                    new MealPlanRow(MealType.Dinner, dinnerPercentage * TotalCalories)
                }.Concat(CreateSnacks(MealsAmount - 3, snackPercentage * TotalCalories)).ToArray()
            };
        }

        private static IEnumerable<MealPlanRow> CreateSnacks(int amount, double calories)
        {
            var snacks = new List<MealPlanRow>();
            for (var i = 0; i < amount; i++)
                snacks.Add(new MealPlanRow(MealType.Snack, calories));

            return snacks.ToArray();
        }

        public void Recalculate()
        {
            var expectedCalories = 0.0;
            var actualCalories = 0.0;

            foreach (var row in MealPlanRows)
                if (row.Recipe != null)
                {
                    expectedCalories += Convert.ToDouble(row.Calories);
                    actualCalories += Convert.ToDouble(row.Recipe.Calories);
                    row.Calories = row.Recipe.Calories;
                }
                else
                {
                    row.Calories = Convert.ToInt32(row.Calories / (TotalCalories - expectedCalories) *
                                                   (TotalCalories - actualCalories));
                }
        }

        public void StripMetadata()
        {
            MealPlanRows = MealPlanRows.Select(x =>
            {
                x.Recipe.Metadata = null;
                return x;
            }).ToArray();
        }
    }
}