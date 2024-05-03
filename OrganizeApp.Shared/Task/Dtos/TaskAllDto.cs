using OrganizeApp.Shared.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = OrganizeApp.Shared.Common.Enums.TaskStatus;

namespace OrganizeApp.Shared.Task.Dtos
{
    public class TaskAllDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public DateTime? DateOfPlannedStart { get; set; }
        public DateTime? DateOfPlannedEnd { get; set; }
        public DateTime? DateOfComplete { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }
}
