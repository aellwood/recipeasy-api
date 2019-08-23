using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Recipeasy_API.Entities;
using Recipeasy_API.Interfaces.Services;
using Recipeasy_API.Payload;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Recipeasy_API.Services
{
    internal class RecipesService : IRecipesService
{
        private readonly string dbPassword;

        public RecipesService(IConfiguration configuration)
        {
            dbPassword = configuration["SecretTablesPassword"];
        }

        public async Task AddRecipe(RecipePayload recipePayload, string email)
        {
            var recipeTable = await AccessDb();

            var recipeEntity = new RecipeEntity(email, recipePayload.RecipeName)
            {
                Ingredients = recipePayload.Ingredients.ToString(Formatting.None),
                Notes = recipePayload.Notes
            };

            await recipeTable.ExecuteAsync(TableOperation.Insert(recipeEntity));
        }

        public async Task GetRecipes(string email)
        {
            var recipeTable = await AccessDb();

            // await recipeTable.ExecuteAsync(TableOperation.Retrieve("")));
        }

        private async Task<CloudTable> AccessDb()
        {
            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("recipeasytables", dbPassword),
                true
            );

            var tableClient = storageAccount.CreateCloudTableClient();

            var recipeTable = tableClient.GetTableReference("recipeTable");

            await recipeTable.CreateIfNotExistsAsync();

            return recipeTable;
        }
    }
}
