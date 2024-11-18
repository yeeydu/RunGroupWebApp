﻿using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace RunGroupWebApp
{
    public static class ClaimPrincipalExtension
    {

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
