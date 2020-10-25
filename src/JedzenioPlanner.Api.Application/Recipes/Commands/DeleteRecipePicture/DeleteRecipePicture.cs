using System;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipePicture
{
    public class DeleteRecipePicture : IRequest
    {
        public Guid Id { get; set; }
    }
}