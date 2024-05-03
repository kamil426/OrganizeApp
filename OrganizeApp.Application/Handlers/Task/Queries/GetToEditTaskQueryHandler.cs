using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Queries
{
    public class GetToEditTaskQueryHandler : IRequestHandler<GetToEditTaskQuery, EditTaskCommand>
    {
        private IApplicationDbContext _context;

        public GetToEditTaskQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditTaskCommand> Handle(GetToEditTaskQuery request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (task == null)
                return null;

            return new EditTaskCommand
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DateOfPlannedStart = task.DateOfPlannedStart,
                DateOfPlannedEnd = task.DateOfPlannedEnd,
            };
        }
    }
}
