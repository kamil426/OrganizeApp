using MediatR;
using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Task.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Category.Queries
{
    public class GetToEditCategoryQuery : IRequest<EditCategoryCommand>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}
