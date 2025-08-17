using MediatR;
using OrganizeApp.Shared.Task.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Queries
{
    public class GetTasksCheckListQuery : IRequest<IEnumerable<TasksCheckListDto>>
    {
        public string UserId { get; set; }
    }
}
