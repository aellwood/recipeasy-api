using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Models;
using Recipeasy.Api.Extensions;

namespace Recipeasy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRecipesService _recipeService;

        public RecipesController(IConfiguration configuration, IRecipesService recipeService)
        {
            _configuration = configuration;
            _recipeService = recipeService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Recipe recipe)
        {
            var addedRecipe = _recipeService.AddRecipe(recipe, GetUserId());

            return Ok(addedRecipe);
        }

        [HttpGet]
        [Authorize]
        [Route("{recipeId}")]
        public IActionResult Get([FromRoute] string recipeId)
        {
            var recipe = _recipeService.GetRecipe(GetUserId(), recipeId);

            return Ok(recipe);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetMany()
        {
            var recipes = _recipeService.GetRecipes(GetUserId());

            return Ok(recipes);
        }

        [HttpDelete]
        [Authorize]
        [Route("{recipeId}")]
        public IActionResult Delete([FromRoute] string recipeId)
        {
            var recipe = _recipeService.DeleteRecipe(GetUserId(), recipeId);

            return Ok(recipe);
        }

        private string GetUserId()
        {
            return HttpContext.GetClaimValue(_configuration["Auth0ClaimNameSpace"]);
        }
    }
}
