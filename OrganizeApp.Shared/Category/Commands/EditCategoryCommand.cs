using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Category.Commands
{
    public class EditCategoryCommand : IRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Nazwa kategorii' jest wymagane")]
        [MaxLength(40, ErrorMessage = $"Pole 'Nazwa kategorii' może mieć maksymalnie 40 znaków")]
        public string Name { get; set; }
        public string? Color { get; set; }
        public string UserId { get; set; }
    }
}
