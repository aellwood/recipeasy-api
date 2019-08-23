using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recipeasy_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "AnonymousAllowed", "Becca", "Colin", "Matt", "Andrew" };
        }

        [HttpGet, Route("testGet"), Authorize]
        public ActionResult<IEnumerable<string>> AuthorisedGet()
        {
            return new string[] { "Authorised", "Andrew", "Colin", "Rebecca" };
        }
    }
}
