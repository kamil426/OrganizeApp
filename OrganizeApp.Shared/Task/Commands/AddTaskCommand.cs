using MediatR;
using OrganizeApp.Shared.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OrganizeApp.Shared.Task.Commands
{
    public class AddTaskCommand : IRequest
    {
        [Required(ErrorMessage = "Pole 'Tytuł' jest wymagane")]
        [MaxLength(100, ErrorMessage =$"Pole 'Tytuł' może mieć maksymalnie 100 znaków")]
        public string Title { get; set; }

        [MaxLength(1250, ErrorMessage = "Pole 'Opis' może mieć maksymalnie 1250 znaków")]
        public string Description { get; set; }
        public DateTime? DateOfPlannedStart { get; set; }

        [DateGreaterThan("DateOfPlannedStart", ErrorMessage = "Data zakończenia musi być większa od daty rozpoczęcia")]
        public DateTime? DateOfPlannedEnd { get; set; }

        [Range(1, 2, ErrorMessage = "Pole 'Status zadania' jest wymagane")]
        public Common.Enums.TaskStatus TaskStatus { get; set; }
    }
}
