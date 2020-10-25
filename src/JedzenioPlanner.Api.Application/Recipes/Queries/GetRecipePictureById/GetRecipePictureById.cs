using System;
using JedzenioPlanner.Api.Domain.Filestore;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipePictureById
{
    public class GetRecipePictureById : IRequest<StoredFile>
    {
        public Guid Id { get; set; }
    }
}