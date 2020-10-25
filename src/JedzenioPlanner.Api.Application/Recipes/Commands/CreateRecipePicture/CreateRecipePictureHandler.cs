using System;
using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipePicture
{
    public class CreateRecipePictureHandler : IRequestHandler<CreateRecipePicture, Guid>
    {
        private readonly IFileStore _store;
        
        public CreateRecipePictureHandler(IFileStore store)
        {
            _store = store;
        }
        
        public async Task<Guid> Handle(CreateRecipePicture request, CancellationToken cancellationToken)
        {
            return await _store.CreateFile(request.Content);
        }
    }
}