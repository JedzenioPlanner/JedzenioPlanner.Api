using System;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipePicture;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Models.Recipe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JedzenioPlanner.Api.Controllers.Admin
{
    /// <summary>
    /// Recipes controller (for admin)
    /// </summary>
    [Route("/admin/recipes")]
    [ApiController]
    [Authorize]
    public class AdminRecipeController : Controller
    {
        private readonly IMediator _mediator;

        public AdminRecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets an existing recipe (with metadata)
        /// </summary>
        /// <param name="id">Recipe id</param>
        /// <returns>Requested recipe</returns>
        /// <response code="200">When the recipe is found and returned</response>
        /// <response code="400">When validation error occurs</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Recipe> GetById(Guid id)
        {
            return await _mediator.Send(new GetRecipeById(id, false));
        }

        /// <summary>
        /// Update a recipe
        /// </summary>
        /// <param name="id">Recipe id</param>
        /// <param name="model">Recipe model</param>
        /// <returns>An info whether update was successful</returns>
        /// <response code="200">When the recipe is updated successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, UpdateRecipeModel model)
        {
            await _mediator.Send(model.Map(id));
            return Ok();
        }

        /// <summary>
        /// Delete a recipe
        /// </summary>
        /// <param name="id">Recipe id</param>
        /// <returns>An info whether deletion was successful</returns>
        /// <response code="200">When the recipe is deleted successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteRecipe(id));
            return Ok();
        }
        
        /// <summary>
        /// Restores deleted recipe
        /// </summary>
        /// <param name="id">Recipe id</param>
        /// <returns>An info whether restoration was successful</returns>
        /// <response code="200">When the recipe is found and restored</response>
        /// <response code="400">When validation error occurs</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RestoreRecipe(Guid id)
        {
            await _mediator.Send(new RestoreRecipe{EntityId = id});
            return Ok();
        }

        /// <summary>
        /// Delete a picture
        /// </summary>
        /// <param name="id">Recipe picture id</param>
        /// <returns>An info whether deletion was successful</returns>
        /// <response code="200">When the picture is found and deleted successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpDelete("pictures/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePicture(Guid id)
        {
            await _mediator.Send(new DeleteRecipePicture {Id = id});
            return Ok();
        }
    }
} 