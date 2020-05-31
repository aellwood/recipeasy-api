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

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] Recipe recipe)
        {
            var id = await recipeService.AddRecipe(recipe, GetUserEmail());

            return Ok(id);
        }

        [HttpGet, Authorize]
        public IActionResult Get()
        {
            var recipes = recipeService.GetRecipes(GetUserEmail());

            return Ok(recipes);
        }

        [HttpDelete, Authorize]
        public IActionResult Delete([FromBody] string recipeId)
        {
            var recipe = recipeService.DeleteRecipe(GetUserEmail(), recipeId);

            return Ok(recipe);
        }

        private string GetUserEmail()
        {
            return HttpContext.GetClaimValue(configuration["Auth0ClaimNameSpace"] + "/email");
        }
    }
}
