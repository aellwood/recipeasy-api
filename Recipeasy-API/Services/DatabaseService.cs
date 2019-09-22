using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Recipeasy_API.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Services
{
    internal class DatabaseService : IDatabaseService
    {
        private readonly string dbPassword;

        public DatabaseService(IConfiguration configuration)
        {
            dbPassword = configuration["SecretTablesPassword"];
        }

        public async Task Add<T>(string email, T entity) where T : TableEntity, new()
        {
            var table = await GetTable("recipeTable");

            await table.ExecuteAsync(TableOperation.Insert(entity));
        }

        public async Task<IEnumerable<T>> Get<T>(string email) where T : TableEntity, new()
        {
            var table = await GetTable("recipeTable");
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, email));

            TableContinuationToken token = null;
            do
            {
                var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                return resultSegment.Results;
            } while (token != null);
        }

        public async Task<T> Delete<T>(string email, string id) where T : TableEntity, new()
        {
            var table = await GetTable("recipeTable");
            var retrieveOperation = TableOperation.Retrieve<T>(email, id);

            var row = await table.ExecuteAsync(retrieveOperation);

            if (row != null)
            {
                var deleteEntity = (T) row.Result;
                await table.ExecuteAsync(TableOperation.Delete(deleteEntity));
                return deleteEntity;
            }

            return null;
        }

        private async Task<CloudTable> GetTable(string tableName)
        {
            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("recipeasytables", dbPassword),
                true
            );

            var tableClient = storageAccount.CreateCloudTableClient();

            var recipeTable = tableClient.GetTableReference(tableName);

            await recipeTable.CreateIfNotExistsAsync();

            return recipeTable;
        }
    }
}
