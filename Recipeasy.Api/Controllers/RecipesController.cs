using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipeasy.Api.ExtensionMethods;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Models;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Post([FromBody] Recipe recipe)
        {
            var addedRecipe = await _recipeService.AddRecipe(recipe, GetUserId());

            return Ok(addedRecipe);
        }

        [HttpGet]
        [Authorize]
        [Route("{recipeId}")]
        public async Task<IActionResult> Get([FromRoute] string recipeId)
        {
            var recipe = await _recipeService.GetRecipe(GetUserId(), recipeId);

            return Ok(recipe);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMany()
        {
            var recipes = await _recipeService.GetRecipes(GetUserId());

            return Ok(recipes);
        }

        [HttpGet]
        [Route("test")]
        public IActionResult TestGet()
        {
            return Ok("Successful GET request");
        }

        [HttpDelete]
        [Authorize]
        [Route("{recipeId}")]
        public async Task<IActionResult> Delete([FromRoute] string recipeId)
        {
            var recipe = await _recipeService.DeleteRecipe(GetUserId(), recipeId);

            return Ok(recipe);
        }

        private string GetUserId()
        {
            return HttpContext.GetClaimValue(_configuration["Auth0ClaimNameSpace"]);
        }
    }
}
