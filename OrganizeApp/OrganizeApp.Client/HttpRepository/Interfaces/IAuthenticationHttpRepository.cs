using OrganizeApp.Shared.Authentication.Commands;
using OrganizeApp.Shared.Authentication.Dtos;
using OrganizeApp.Shared.Common.Models;
using System.Net;

namespace OrganizeApp.Client.HttpRepository.Interfaces
{
    public interface IAuthenticationHttpRepository
    {
        Task<string> RefreshToken();
        Task<ResponseDto> RegisterUser(RegisterUserCommand registerUserCommand);
        Task<HttpStatusCode> EmailConfirmation(string email, string token);
        Task<LoginUserDto> Login(LoginUserCommand userForAuthentication);
        Task Logout();
        Task<HttpStatusCode> ForgotPassword(ForgotPasswordCommand command);
        Task<ResponseDto> ResetPassword(ResetPasswordCommand command);
    }
}
