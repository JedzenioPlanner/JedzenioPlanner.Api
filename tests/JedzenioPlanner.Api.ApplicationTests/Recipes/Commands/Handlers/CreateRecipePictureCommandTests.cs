using System;
using System.Threading;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipePicture;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Handlers
{
    public class CreateRecipePictureCommandTests
    {
        [Fact]
        public async void ShouldCreateRecipePictureCorrectly()
        {
            var samplePicture = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAIAAAACUFjqAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAOSURBVChTYxgFJAMGBgABNgABY8OiGAAAAABJRU5ErkJggg==");
            var fileStoreMock = MockBuilder.BuildFakeFileStore();
            var request = new CreateRecipePicture {Content = samplePicture};
            var handler = new CreateRecipePictureHandler(fileStoreMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            fileStoreMock.Verify(x => x.CreateFile(samplePicture));
        }
    }
}