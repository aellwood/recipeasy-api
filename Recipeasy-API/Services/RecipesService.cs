using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
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
    internal class RecipesService : IRecipesService
{
        private readonly string dbPassword;

        public RecipesService(IConfiguration configuration)
        {
            dbPassword = configuration["SecretTablesPassword"];
        }

        public async Task<RecipeModel> AddRecipe(RecipeModel recipePayload, string email)
        {
            var recipeTable = await AccessDb();

            var guid = Guid.NewGuid().ToString();

            var recipeEntity = new RecipeEntity(email, guid)
            {
                RecipeName = recipePayload.RecipeName,
                Ingredients = recipePayload.Ingredients.ToString(Formatting.None),
                Notes = recipePayload.Notes
            };

            await recipeTable.ExecuteAsync(TableOperation.Insert(recipeEntity));

            recipePayload.RecipeId = guid;
            return recipePayload;
        }

        public async Task<IEnumerable<RecipeModel>> GetRecipes(string email)
        {
            var recipeTable = await AccessDb();
            var recipeEntities = await GetRecipes(recipeTable, email);

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
            var table = await AccessDb();

            var retrieveOperation = TableOperation.Retrieve<RecipeEntity>(email, recipeId);
            var recipe = await table.ExecuteAsync(retrieveOperation);
            var deleteEntity = (RecipeEntity) recipe.Result;

            if (recipe != null)
            {
                await table.ExecuteAsync(TableOperation.Delete(deleteEntity));
                return deleteEntity;
            }

            return null;
        }

        private async Task<IEnumerable<RecipeEntity>> GetRecipes(CloudTable recipeTable, string email)
        {
            var query = new TableQuery<RecipeEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, email));

            TableContinuationToken token = null;
            do
            {
                var resultSegment = await recipeTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                return resultSegment.Results;
            } while (token != null);
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
