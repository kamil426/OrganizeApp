using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OrganizeApp.Shared.Authentication.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        [Required(ErrorMessage = "Pole 'Nowe hasło' jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków i nie więcej niż {1} znaków długości.", MinimumLength = 8)]
        public string Password { get; set; }
        [Compare(nameof(Password),
            ErrorMessage = "Hasło i Potwierdzone hasło są różne.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
