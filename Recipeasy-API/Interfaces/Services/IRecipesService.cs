using Recipeasy_API.Payload;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IRecipesService
    {
        Task AddRecipe(RecipePayload recipe, string email);
        Task GetRecipes(string email);
    }
}
