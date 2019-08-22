using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet, Route("testGet"), Authorize]
        public ActionResult<IEnumerable<string>> Get2()
        {
            return new string[] { "Andrew", "Colin", "Rebecca" };
        }

        // POST api/values
        [HttpPost, Authorize]
        public async void Post([FromBody] string recipeName)
        {
            var password = _configuration.GetValue<string>("SecretTablesPassword");

            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("recipeasytables", password),
                true
                );

            var tableClient = storageAccount.CreateCloudTableClient();

            var recipeTable = tableClient.GetTableReference("recipeTable");

            await recipeTable.CreateIfNotExistsAsync();

            var recipe = new RecipeEntity("testUsername", recipeName);

            await recipeTable.ExecuteAsync(TableOperation.Insert(recipe));
        }
    }
}
