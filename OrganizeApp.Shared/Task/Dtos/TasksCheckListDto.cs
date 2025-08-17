using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Dtos
{
    public class TasksCheckListDto
    {
        public string Title { get; set; }
        public Common.Enums.TaskStatus TaskStatus { get; set; }
    }
}
