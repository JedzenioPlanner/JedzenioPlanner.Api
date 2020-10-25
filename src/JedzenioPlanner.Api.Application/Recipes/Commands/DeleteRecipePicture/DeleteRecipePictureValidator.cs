using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JedzenioPlanner.Api.Application.Common.Interfaces;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipePicture
{
    public class DeleteRecipePictureValidator : AbstractValidator<DeleteRecipePicture>
    {
        private readonly IFileStore _store;
        
        public DeleteRecipePictureValidator(IFileStore store)
        {
            _store = store;
            
            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(Exist);
        }

        private async Task<bool> Exist(Guid id, CancellationToken cancellationToken)
        {
            var file = await _store.ReadFile(id);

            return file?.Length != 0;
        }
    }
}