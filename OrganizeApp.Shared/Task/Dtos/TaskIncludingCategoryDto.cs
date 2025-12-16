using OrganizeApp.Shared.Category.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Dtos
{
    public class TaskIncludingCategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateOfPlannedStart { get; set; }
        public DateTime? DateOfPlannedEnd { get; set; }
        public DateTime? DateOfComplete { get; set; }
        public Shared.Common.Enums.TaskStatus TaskStatus { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDto? CategoryDto { get; set; }
    }
}
