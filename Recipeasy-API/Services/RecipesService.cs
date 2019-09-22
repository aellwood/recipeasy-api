using Recipeasy_API.Entities;
using Recipeasy_API.Interfaces.Services;
using Recipeasy_API.Payload;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;

namespace Recipeasy_API.Services
{
    public class RecipesService : IRecipesService
{
        private readonly IDatabaseService databaseService;

        public RecipesService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public async Task<RecipeModel> AddRecipe(RecipeModel recipePayload, string email)
        {
            var guid = Guid.NewGuid().ToString();

            var recipeEntity = new RecipeEntity(email, guid)
            {
                RecipeName = recipePayload.RecipeName,
                Ingredients = recipePayload.Ingredients.ToString(Formatting.None),
                Notes = recipePayload.Notes
            };

            await databaseService.Add(email, recipeEntity);

            recipePayload.RecipeId = guid;
            return recipePayload;
        }

        public async Task<IEnumerable<RecipeModel>> GetRecipes(string email)
        {
            var recipeEntities = await databaseService.Get<RecipeEntity>(email);

            return recipeEntities.Select(x => new RecipeModel
            {
                RecipeId = x.RowKey,
                RecipeName = x.RecipeName,
                Ingredients = JArray.Parse(x.Ingredients),
                Notes = x.Notes
            });
        }

        public async Task<RecipeEntity> DeleteRecipe(string email, string recipeId)
        {
            return await databaseService.Delete<RecipeEntity>(email, recipeId);
        }
    }
}
