using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Commands
{
    public class DeleteTaskCommand : IRequest
    {
        public int Id { get; set; }
    }
}
