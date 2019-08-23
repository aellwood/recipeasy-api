using Recipeasy_API.Payload;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IRecipesService
    {
        Task AddRecipe(RecipePayload recipe, string email);
        Task<IEnumerable<RecipePayload>> GetRecipes(string email);
    }
}
