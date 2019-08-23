using Recipeasy_API.Payload;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IRecipesService
    {
        Task<RecipeModel> AddRecipe(RecipeModel recipe, string email);
        Task<IEnumerable<RecipeModel>> GetRecipes(string email);
    }
}
