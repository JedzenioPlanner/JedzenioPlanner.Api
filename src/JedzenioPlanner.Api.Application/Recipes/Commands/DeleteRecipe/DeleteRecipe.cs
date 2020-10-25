using System;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipe : IRequest
    {
        public DeleteRecipe(Guid id)
        {
            EntityId = id;
        }
        
        public Guid EntityId { get; set; }
    }
}