using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Domain.Recipes
{
    public class Recipe : AggregateRoot
    {
        public Recipe()
        {
            MealTypes = ArraySegment<MealType>.Empty;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int Calories { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<string> Steps { get; set; }
        public IEnumerable<MealType> MealTypes { get; set; }
    }
}