using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetUserRecipes
{
    public class GetUserRecipes : IRequest<IEnumerable<Recipe>>
    {
        public GetUserRecipes(string userId, int page = 0, int amount = 10)
        {
            UserId = userId;
            Page = page;
            Amount = amount;
        }

        public string UserId { get; set; }
        public int Page { get; set; }
        public int Amount { get; set; }
    }
}