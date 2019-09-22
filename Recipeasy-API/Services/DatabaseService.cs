using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Recipeasy_API.Helpers;
using Recipeasy_API.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string dbPassword;

        public DatabaseService(IConfiguration configuration)
        {
            dbPassword = configuration["SecretTablesPassword"];
        }

        public async Task Add<T>(string email, T entity) where T : TableEntity, new()
        {
            var tableName = TableNameHelper.GetTableName(typeof(T).Name);
            var table = await GetTable(tableName);
            await table.ExecuteAsync(TableOperation.Insert(entity));
        }

        public async Task<IEnumerable<T>> Get<T>(string email) where T : TableEntity, new()
        {
            var tableName = TableNameHelper.GetTableName(typeof(T).Name);
            var table = await GetTable(tableName);
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, email));

            TableContinuationToken token = null;

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;

                return segment.Results;
            }
            while (token != null);
        }

        public async Task<T> Delete<T>(string email, string id) where T : TableEntity, new()
        {
            var tableName = TableNameHelper.GetTableName(typeof(T).Name);
            var table = await GetTable(tableName);
            var op = TableOperation.Retrieve<T>(email, id);
            var row = await table.ExecuteAsync(op);

            if (row != null)
            {
                var entity = (T) row.Result;
                await table.ExecuteAsync(TableOperation.Delete(entity));
                return entity;
            }

            return null;
        }

        private async Task<CloudTable> GetTable(string tableName)
        {
            var acc = new CloudStorageAccount(new StorageCredentials("recipeasytables", dbPassword), true);
            var client = acc.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
