using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRandomRecipes
{
    public class GetRandomRecipes : IRequest<IEnumerable<Guid>>
    {
    }
}