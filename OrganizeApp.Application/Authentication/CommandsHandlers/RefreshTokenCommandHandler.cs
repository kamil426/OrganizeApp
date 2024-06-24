using MediatR;
using Microsoft.AspNetCore.Identity;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Shared.Authentication.Commands;
using OrganizeApp.Shared.Authentication.Dtos;

namespace OrganizeApp.Application.Authentication.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginUserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _authenticationService;

        public RefreshTokenCommandHandler(
            UserManager<ApplicationUser> userManager,
            IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        public async Task<LoginUserDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _authenticationService
                .GetPrincipalFromExpiredToken(request.Token);
            var name = principal.Identity.Name;

            var user = await _userManager.FindByEmailAsync(name);

            if (user == null || user.Email != name)
                return new LoginUserDto { ErrorMessage = "Nieprawidłowe dane" };

            if (user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                return new LoginUserDto { ErrorMessage = "Nieprawidłowe dane" };

            var token = await _authenticationService.GetToken(user);

            await _userManager.UpdateAsync(user);

            return new LoginUserDto
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                IsAuthSuccessful = true
            };
        }
    }
}
