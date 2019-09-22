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

        public async Task<CloudTable> AccessDb()
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

        public async Task<IEnumerable<T>> Get<T>(CloudTable table, string email) where T : TableEntity, new()
        {
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, email));

            TableContinuationToken token = null;
            do
            {
                var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                return resultSegment.Results;
            } while (token != null);
        }
    }
}
