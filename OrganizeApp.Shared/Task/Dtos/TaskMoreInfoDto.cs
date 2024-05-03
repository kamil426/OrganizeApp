using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Dtos
{
    public class TaskMoreInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfPlannedStart { get; set; }
        public DateTime? DateOfPlannedEnd { get; set; }
        public DateTime? DateOfComplete { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }
}
