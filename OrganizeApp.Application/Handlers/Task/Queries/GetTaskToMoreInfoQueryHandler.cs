using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using OrganizeApp.Shared.Task.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Queries
{
    public class GetTaskToMoreInfoQueryHandler : IRequestHandler<GetMoreInfoTaskQuery, TaskMoreInfoDto>
    {
        private IApplicationDbContext _context;

        public GetTaskToMoreInfoQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskMoreInfoDto> Handle(GetMoreInfoTaskQuery request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.SingleAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (task == null)
                return null;

            return new TaskMoreInfoDto
            {
                Title = task.Title,
                Description = task.Description,
                DateOfPlannedStart = task.DateOfPlannedStart,
                DateOfPlannedEnd = task.DateOfPlannedEnd,
                DateOfComplete = task.DateOfComplete,
                TaskStatus = task.TaskStatus,
            };
        }
    }
}
