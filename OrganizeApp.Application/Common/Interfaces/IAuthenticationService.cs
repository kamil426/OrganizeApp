using Microsoft.AspNetCore.Identity;
using OrganizeApp.Domain.Entities;
using System.Security.Claims;

namespace OrganizeApp.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
