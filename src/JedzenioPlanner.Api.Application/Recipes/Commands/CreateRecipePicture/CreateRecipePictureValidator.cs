using FluentValidation;
using SixLabors.ImageSharp;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipePicture
{
    public class CreateRecipePictureValidator : AbstractValidator<CreateRecipePicture>
    {
        public CreateRecipePictureValidator()
        {
            RuleFor(x => x)
                .Must(BeAPicture);
            RuleFor(x => x.Content)
                .Transform(x => x.Length)
                .Must(BeInSizeLimit);
        }

        private static bool BeAPicture(CreateRecipePicture request)
        {
            try
            {
                var image = Image.Load(request.Content);
                image.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool BeInSizeLimit(int length)
            => length < 10485760;
    }
}