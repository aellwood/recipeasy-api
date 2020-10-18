using Recipeasy.Api.Models;
using System.Collections.Generic;

namespace Recipeasy.Api.Interfaces.Services
{
    public interface IRecipesService
    {
        Recipe AddRecipe(Recipe recipe, string userId);
        Recipe GetRecipe(string userId, string recipeId);
        List<Recipe> GetRecipes(string userId);
        Recipe DeleteRecipe(string userId, string recipeId);
    }
}
