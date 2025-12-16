using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Category.Commands
{
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}
