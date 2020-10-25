using System;
using System.Threading;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById;
using JedzenioPlanner.Api.Controllers.Admin;
using MediatR;
using Moq;
using Xunit;

namespace JedzenioPlanner.ApiTests.Controllers.Admin
{
    public class RecipeControllerTests
    {
        [Fact]
        public async void ShouldGetByIdCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new AdminRecipeController(mediatorMock.Object);

            await controller.GetById(new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97"));

            mediatorMock.Verify(x => x.Send(It.Is<GetRecipeById>(y => y.Id == new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97") && y.StripMetadata == false), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldRestoreRecipeCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new AdminRecipeController(mediatorMock.Object);

            await controller.RestoreRecipe(new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97"));

            mediatorMock.Verify(x => x.Send(It.Is<RestoreRecipe>(y => y.EntityId == new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97")), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.VerifyNoOtherCalls();
        }
    }
}