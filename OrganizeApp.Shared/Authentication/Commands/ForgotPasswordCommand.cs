using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OrganizeApp.Shared.Authentication.Commands
{
    public class ForgotPasswordCommand : IRequest
    {
        [Required(ErrorMessage = "Pole 'E-mail' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'E-mail' musi być prawidłowym adresem e-mail.")]
        public string Email { get; set; }
        public string ClientURI { get; set; }
    }
}
