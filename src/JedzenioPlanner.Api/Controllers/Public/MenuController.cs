using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Menus.Queries.FindRecipeQuery;
using JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JedzenioPlanner.Api.Controllers.Public
{
    /// <summary>
    ///     MenuGenerator controller
    /// </summary>
    [Route("/menu")]
    [ApiController]
    public class MenuController : Controller
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Generate random menu
        /// </summary>
        /// <returns>Array with recipes</returns>
        /// <response code="200">When menu is returned successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateRandomMenu(int caloriesTarget, int mealsAmount,
            int breakfastPercentage, int lunchPercentage, int dinnerPercentage, int snackPercentage)
        {
            return Ok(
                await _mediator.Send(
                    new GenerateRandomMenu(caloriesTarget, mealsAmount, breakfastPercentage, lunchPercentage,
                        dinnerPercentage, snackPercentage)
                ));
        }

        /// <summary>
        ///     Find a single recipe
        /// </summary>
        /// <returns>Single recipe</returns>
        /// <response code="200">When menu is returned successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpGet("single")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindRecipe(MealType type, int calories)
        {
            return Ok(await _mediator.Send(new FindRecipe(type, calories)));
        }
    }
}