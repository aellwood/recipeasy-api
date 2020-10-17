using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Recipeasy.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static Claim GetClaim(this HttpContext ctx, string claimType)
        {
            return ctx.User.Claims.FirstOrDefault(x => x.Type == claimType);
        }

        public static string GetClaimValue(this HttpContext ctx, string claimType)
        {
            return GetClaim(ctx, claimType).Value;
        }
    }
}
