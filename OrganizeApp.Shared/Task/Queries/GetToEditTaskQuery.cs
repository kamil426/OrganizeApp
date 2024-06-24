using MediatR;
using OrganizeApp.Shared.Task.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Task.Queries
{
    public class GetToEditTaskQuery : IRequest<EditTaskCommand>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}
