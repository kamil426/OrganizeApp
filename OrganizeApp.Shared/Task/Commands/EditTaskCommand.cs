using MediatR;
using OrganizeApp.Shared.Common.Enums;
using OrganizeApp.Shared.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Commands
{
    public class EditTaskCommand : IRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Tytuł' jest wymagane")]
        [MaxLength(100, ErrorMessage = "Pole 'Tytuł' może mieć maksymalnie 100 znaków")]
        public string Title { get; set; }

        [MaxLength(1250, ErrorMessage = "Pole 'Opis' może mieć maksymalnie 1250 znaków")]
        public string Description { get; set; }
        public DateTime? DateOfPlannedStart { get; set; }

        [DateGreaterThan("DateOfPlannedStart", ErrorMessage = "Data zakończenia musi być większa od daty rozpoczęcia")]
        public DateTime? DateOfPlannedEnd { get; set; }
        public string UserId { get; set; }
    }
}
