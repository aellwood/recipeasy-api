using System.Collections.Generic;
using System.Threading.Tasks;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Models;

namespace Recipeasy.Api.Services.V2
{
    public class RecipesV2Service : IRecipesService
    {
        public Task<Recipe> AddRecipe(Recipe recipe, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Recipe> GetRecipe(string userId, string recipeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Recipe>> GetRecipes(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Recipe> DeleteRecipe(string v, string recipeId)
        {
            throw new System.NotImplementedException();
        }
    }
}