using MediatR;
using OrganizeApp.Shared.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Commands
{
    public class ChangeStatusTaskCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime? DateOfComplete { get; set; }
        public Common.Enums.TaskStatus TaskStatus { get; set; }
    }
}
