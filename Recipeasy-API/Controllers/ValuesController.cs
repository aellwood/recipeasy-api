using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Recipeasy_API.Entities;

namespace Recipeasy_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Andrew", "Colin", "Rebecca" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return _configuration["AppSecret"];
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] string recipeName)
        {
            var password = _configuration.GetValue<string>("SecretValues:SecretTablesPassword");

            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("recipeasytables", password),
                true
                );

            var tableClient = storageAccount.CreateCloudTableClient();

            var recipeTable = tableClient.GetTableReference("recipeTable");

            await CreatePeopleTableAsync(recipeTable);

            var recipe = new RecipeEntity("testUsername", recipeName);

            await recipeTable.ExecuteAsync(TableOperation.Insert(recipe));
        }

        private async Task CreatePeopleTableAsync(CloudTable recipeTable)
        {
            // Create the CloudTable if it does not exist
            await recipeTable.CreateIfNotExistsAsync();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
