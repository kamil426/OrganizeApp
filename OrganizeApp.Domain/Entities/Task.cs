using OrganizeApp.Shared.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizeApp.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfPlannedStart { get; set; }
        public DateTime? DateOfPlannedEnd { get; set; }
        public DateTime? DateOfComplete { get; set; }
        public Shared.Common.Enums.TaskStatus TaskStatus { get; set; }
    }
}
