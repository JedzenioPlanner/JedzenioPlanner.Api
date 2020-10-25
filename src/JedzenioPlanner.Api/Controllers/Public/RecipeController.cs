using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipePicture;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetAllRecipes;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRandomRecipes;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipePictureById;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetUserRecipes;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Models.Recipe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JedzenioPlanner.Api.Controllers.Public
{
    /// <summary>
    /// Recipes controller
    /// </summary>
    [Route("recipes")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets recipes
        /// </summary>
        /// <returns>Array with selected amount of recipes from the database</returns>
        /// <response code="200">When recipes are returned successfully</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Recipe>> GetAll(int page = 0, int amount = 10)
        {
            return await _mediator.Send(new GetRecipes(page, amount));
        }

        /// <summary>
        /// Gets an existing recipe
        /// </summary>
        /// <param name="id">Recipe id</param>
        /// <returns>Requested recipe</returns>
        /// <response code="200">When the recipe is found and returned</response>
        /// <response code="400">When validation error occurs</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Recipe> GetById(Guid id)
        {
            return await _mediator.Send(new GetRecipeById(id));
        }

        /// <summary>
        /// Gets recipes created by given user
        /// </summary>
        /// <returns>Array with recipes found in the database</returns>
        /// <response code="200">When recipes are returned successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<Recipe>> GetUserRecipes(string id, int page = 0, int amount = 10)
        {
            return await _mediator.Send(new GetUserRecipes(id, page, amount));
        }

        /// <summary>
        /// Gets random recipes ids
        /// </summary>
        /// <returns>Array with recipe ids found in the database</returns>
        /// <response code="200">When recipe ids are returned successfully</response>
        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Guid>> GetRandomRecipesIds()
        {
            return await _mediator.Send(new GetRandomRecipes());
        }
        
        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="model">Recipe model</param>
        /// <response code="200">When the recipe is created correctly</response>
        /// <response code="400">When validation error occurs</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(CreateRecipeModel model)
        {
            await _mediator.Send(model.Map());
            return Ok();
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
        [Authorize]
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteRecipe(id));
            return Ok();
        }
        
        /// <summary>
        /// Get a recipe picture
        /// </summary>
        /// <param name="id">Recipe picture id</param>
        /// <returns>Requested pictrue</returns>
        /// <response code="200">When the picture is found and returned successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpGet("pictures/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReadPicture(Guid id)
        {
            var file = await _mediator.Send(new GetRecipePictureById {Id = id});
            return new FileContentResult(file.Content, file.Filetype);
        }

        /// <summary>
        /// Upload a picture
        /// </summary>
        /// <param name="file">Picture</param>
        /// <returns>Picture Id</returns>
        /// <response code="200">When the picture is uploaded successfully</response>
        /// <response code="400">When validation error occurs</response>
        [HttpPost("pictures")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<string> UploadPicture(IFormFile file)
        {
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileId = await _mediator.Send(new CreateRecipePicture{Content = ms.ToArray()});
            return fileId.ToString();
        }
    }
} 