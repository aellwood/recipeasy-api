using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipeasy_API.ExtensionMethods;
using Recipeasy_API.Interfaces.Services;
using Recipeasy_API.Models;
using System.Threading.Tasks;

namespace Recipeasy_API.Controllers
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
        public async Task<IActionResult> Get()
        {
            var recipes = await recipeService.GetRecipes(GetUserId());

            return Ok(recipes);
        }

        [HttpDelete]
        [Authorize]
        [Route("{recipeId}")]
        public async Task<IActionResult> Delete([FromRoute] string recipeId)
        {
            await recipeService.DeleteRecipe(GetUserId(), recipeId);

            return Ok();
        }

        private string GetUserId()
        {
            return HttpContext.GetClaimValue(configuration["Auth0ClaimNameSpace"]);
        }
    }
}
