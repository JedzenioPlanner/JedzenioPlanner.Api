using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.Filestore;
using MediatR;
using SixLabors.ImageSharp;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipePictureById
{
    public class GetRecipePictureByIdHandler : IRequestHandler<GetRecipePictureById, StoredFile>
    {
        private readonly IFileStore _store;

        public GetRecipePictureByIdHandler(IFileStore store)
        {
            _store = store;
        }
        
        public async Task<StoredFile> Handle(GetRecipePictureById request, CancellationToken cancellationToken)
        {
            var content = await _store.ReadFile(request.Id);
            Image.Load(content, out var format);
            return new StoredFile
            {
                Id = request.Id,
                Filetype = format.DefaultMimeType,
                Content = content
            };
        }
    }
}