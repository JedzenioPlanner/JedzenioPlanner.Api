using System;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipePicture
{
    public class CreateRecipePicture : IRequest<Guid>
    {
        public byte[] Content { get; set; }
    }
}