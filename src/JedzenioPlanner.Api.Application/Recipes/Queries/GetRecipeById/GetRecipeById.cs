using System;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeById : IRequest<Recipe>
    {
        public GetRecipeById(Guid id, bool stripMetadata = true)
        {
            Id = id;
            StripMetadata = stripMetadata;
        }
        
        public Guid Id { get; set; }
        public bool StripMetadata { get; set; }
    }
}