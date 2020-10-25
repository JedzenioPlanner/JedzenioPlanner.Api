using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetAllRecipes
{
    public class GetRecipes : IRequest<IEnumerable<Recipe>>
    {
        public GetRecipes(int page = 0, int amount = 10)
        {
            Page = page;
            Amount = amount;
        }
        
        public int Page { get; set; }
        public int Amount { get; set; }
    }
}