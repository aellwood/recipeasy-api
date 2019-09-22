using Recipeasy_API.Entities;
using Recipeasy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IRecipesService
    {
        Task AddRecipe(Recipe recipe, string email);
        List<Recipe> GetRecipes(string email);
        Task DeleteRecipe(string v, string recipeId);
    }
}
