using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OrganizeApp.Shared.Authentication.Commands
{
    public class RegisterUserCommand : IRequest
    {
        [Required(ErrorMessage = "Pole 'E-mail' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'E-mail' musi być prawidłowym adresem e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole 'Hasło' jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków i nie więcej niż {1} znaków długości.", MinimumLength = 8)]
        public string Password { get; set; }

        [Compare(nameof(Password),
            ErrorMessage = "Hasło i Potwierdzone hasło są różne.")]
        public string ConfirmPassword { get; set; }
        public string ClientURI { get; set; }

    }
}
