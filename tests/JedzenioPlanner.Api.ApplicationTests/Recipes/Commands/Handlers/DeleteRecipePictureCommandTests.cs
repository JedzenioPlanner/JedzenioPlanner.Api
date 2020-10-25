using System;
using System.Threading;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipePicture;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Handlers
{
    public class DeleteRecipePictureCommandTests
    {
        [Fact]
        public async void ShouldDeleteRecipePictureCorrectly()
        {
            var fileStoreMock = MockBuilder.BuildFakeFileStore();
            var request = new DeleteRecipePicture {Id = new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302")};
            var handler = new DeleteRecipePictureHandler(fileStoreMock.Object);

            await handler.Handle(request, CancellationToken.None);
            fileStoreMock.Verify(x => x.DeleteFile(new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302")));
        }

        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var request = new DeleteRecipePicture {Id = new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302")};
            var validator = new DeleteRecipePictureValidator(MockBuilder.BuildFakeFileStore().Object);

            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void DoesValidatorPreventFromDeletingNonExistingRecipePicture()
        {
            var request = new DeleteRecipePicture {Id = new Guid("DA7F5CDC-FA28-4CC6-90BF-2E7BBE4F885A")};
            var validator = new DeleteRecipePictureValidator(MockBuilder.BuildFakeFileStore().Object);

            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
        }
    }
}