using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy.Api.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task Add<T>(T entity) where T : TableEntity, new();

        Task<T> Get<T>(string partitionKey, string rowKey) where T : TableEntity, new();

        Task<IEnumerable<T>> Get<T>(string partitionKey) where T : TableEntity, new();

        Task<T> Delete<T>(string partitionKey, string rowKey) where T : TableEntity, new();
    }
}
