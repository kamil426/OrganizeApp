using OrganizeApp.Shared.Common.Enums;
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
        public Common.Enums.TaskStatus TaskStatus { get; set; }
    }
}
