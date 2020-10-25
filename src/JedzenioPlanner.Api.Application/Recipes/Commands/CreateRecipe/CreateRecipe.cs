using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipe : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int Calories { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<string> Steps { get; set; }
        public IEnumerable<MealType> MealTypes { get; set; }
    }
}