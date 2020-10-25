using System;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe
{
    public class RestoreRecipe : IRequest
    {
        public Guid EntityId { get; set; }
    }
}