using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Recipeasy_API.ExtensionMethods
{
    public static class HttpContextExtensions
    {
        public static Claim GetClaim(this HttpContext ctx, string claimType)
        {
            return ctx.User.Claims.FirstOrDefault(x => x.Type == claimType);
        }
    }
}
