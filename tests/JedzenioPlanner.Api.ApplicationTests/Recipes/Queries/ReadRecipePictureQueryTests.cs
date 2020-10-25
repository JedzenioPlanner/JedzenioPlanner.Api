using System;
using System.Threading;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipePictureById;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Queries
{
    public class ReadRecipePictureQueryTests
    {
        [Fact]
        public async void ShouldHandleRequestCorrectly()
        {
            var handler = new GetRecipePictureByIdHandler(MockBuilder.BuildFakeFileStore().Object);
            var request = new GetRecipePictureById{Id = new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302")};

            var result = await handler.Handle(request, CancellationToken.None);

            result.Id.Should().Be(new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302"));
        }

        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var validator = new GetRecipePictureByIdValidator(MockBuilder.BuildFakeFileStore().Object);
            var request = new GetRecipePictureById{Id = new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302")};
            
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void DoesValidatorPreventFromReadingNonExistentRecipePicture()
        {
            var validator = new GetRecipePictureByIdValidator(MockBuilder.BuildFakeFileStore().Object);
            var request = new GetRecipePictureById{Id = new Guid("DA7F5CDC-FA28-4CC6-90BF-2E7BBE4F885A")};
            
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
        }
    }
}