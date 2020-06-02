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
        private readonly IConfiguration configuration;
        private readonly IRecipesService recipeService;

        public RecipesController(IConfiguration configuration, IRecipesService recipeService)
        {
            this.configuration = configuration;
            this.recipeService = recipeService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Recipe recipe)
        {
            var addedRecipe = await recipeService.AddRecipe(recipe, GetUserId());

            return Ok(addedRecipe);
        }

        [HttpGet]
        [Authorize]
        [Route("{recipeId}")]
        public async Task<IActionResult> Get([FromRoute] string recipeId)
        {
            var recipe = await recipeService.GetRecipe(GetUserId(), recipeId);

            return Ok(recipe);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMany()
        {
            var recipes = await recipeService.GetRecipes(GetUserId());

            return Ok(recipes);
        }

        [HttpDelete]
        [Authorize]
        [Route("{recipeId}")]
        public async Task<IActionResult> Delete([FromRoute] string recipeId)
        {
            var recipe = await recipeService.DeleteRecipe(GetUserId(), recipeId);

            return Ok(recipe);
        }

        private string GetUserId()
        {
            return HttpContext.GetClaimValue(configuration["Auth0ClaimNameSpace"]);
        }
    }
}
