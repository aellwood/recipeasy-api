using Recipeasy.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy.Api.Interfaces.Services
{
    public interface IRecipesService
    {
        Task<Recipe> AddRecipe(Recipe recipe, string userId);
        Task<Recipe> GetRecipe(string userId, string recipeId);
        Task<List<Recipe>> GetRecipes(string userId);
        Task<Recipe> DeleteRecipe(string v, string recipeId);
    }
}
