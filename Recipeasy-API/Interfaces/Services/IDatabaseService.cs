using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task<CloudTable> AccessDb();

        Task<IEnumerable<T>> Get<T>(CloudTable table, string email) where T : TableEntity, new();
    }
}
