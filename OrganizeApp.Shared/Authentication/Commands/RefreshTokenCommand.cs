using MediatR;
using OrganizeApp.Shared.Authentication.Dtos;

namespace OrganizeApp.Shared.Authentication.Commands
{
    public class RefreshTokenCommand : IRequest<LoginUserDto>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
