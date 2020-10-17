using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Models;
using System.Threading.Tasks;
using Recipeasy.Api.Contexts;
using Recipeasy.Api.Extensions;

namespace Recipeasy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRecipesService _recipeService;
        private readonly ApplicationDbContext _context;

        public RecipesController(IConfiguration configuration, IRecipesService recipeService, ApplicationDbContext context)
        {
            _configuration = configuration;
            _recipeService = recipeService;
            _context = context;
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
            _context.Recipes.Add(new Recipe
            {
                RecipeId = (_context.Recipes.Count() + 1).ToString(),
                RecipeName = "Bla",
                Notes = "Blabla notes"
            });
            
            _context.SaveChanges();
            
            return Ok(_context.Recipes.ToList());
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
