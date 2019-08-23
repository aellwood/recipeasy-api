﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipeasy_API.ExtensionMethods;
using Recipeasy_API.Interfaces.Services;
using Recipeasy_API.Payload;
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
        public async Task<IActionResult> Post([FromBody] RecipePayload recipe)
        {
            await recipeService.AddRecipe(recipe, GetUserEmail());

            return Ok();
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            await recipeService.GetRecipes(GetUserEmail());

            return Ok();
        }

        private string GetUserEmail()
        {
            return HttpContext.GetClaim(configuration["Auth0ClaimNameSpace"] + "/email").Value;
        }
    }
}
