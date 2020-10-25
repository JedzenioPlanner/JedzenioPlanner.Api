using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipePicture
{
    public class DeleteRecipePictureHandler : IRequestHandler<DeleteRecipePicture>
    {
        private readonly IFileStore _store;
        
        public DeleteRecipePictureHandler(IFileStore store)
        {
            _store = store;
        }
        
        public async Task<Unit> Handle(DeleteRecipePicture request, CancellationToken cancellationToken)
        {
            await _store.DeleteFile(request.Id);
            return Unit.Value;
        }
    }
}