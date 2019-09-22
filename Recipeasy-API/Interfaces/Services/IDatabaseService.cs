using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task Add<T>(T entity) where T : TableEntity, new();

        Task<IEnumerable<T>> Get<T>(string email) where T : TableEntity, new();

        Task<T> Delete<T>(string email, string id) where T : TableEntity, new();
    }
}
