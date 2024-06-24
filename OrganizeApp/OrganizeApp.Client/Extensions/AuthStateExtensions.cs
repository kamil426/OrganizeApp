using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace OrganizeApp.Client.Extensions
{
    public static class AuthStateExtensions
    {
        public static string GetUserId(this AuthenticationState authState)
        {
            return authState.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
