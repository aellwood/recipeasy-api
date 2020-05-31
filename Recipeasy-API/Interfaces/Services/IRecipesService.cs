using Recipeasy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IRecipesService
    {
        Task<Recipe> AddRecipe(Recipe recipe, string email);
        Task<List<Recipe>> GetRecipes(string email);
        Task DeleteRecipe(string v, string recipeId);
    }
}
