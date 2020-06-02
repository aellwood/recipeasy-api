﻿using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Recipeasy.Api.ExtensionMethods
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