using System;
using System.Text.Json.Serialization;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Domain.MealPlan
{
    public class MealPlanRow
    {
        public MealPlanRow(MealType type, double targetCalories)
        {
            Type = type;

            TargetCalories = Convert.ToInt32(targetCalories);
            Calories = Convert.ToInt32(targetCalories);
        }

        public MealType Type { get; set; }
        public int TargetCalories { get; set; }

        [JsonIgnore] public int Calories { get; set; }

        public Recipe Recipe { get; set; }
    }
}