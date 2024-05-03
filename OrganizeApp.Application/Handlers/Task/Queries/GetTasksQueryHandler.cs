using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Task.Dtos;
using OrganizeApp.Shared.Task.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Queries
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, IEnumerable<TaskAllDto>>
    {
        private IApplicationDbContext _context;

        public GetTasksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        async Task<IEnumerable<TaskAllDto>> IRequestHandler<GetTasksQuery, IEnumerable<TaskAllDto>>.Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _context
                .Tasks
                .AsNoTracking()
                .OrderBy(x => x.DateOfComplete)
                .Select(x => new TaskAllDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DateOfPlannedStart = x.DateOfPlannedStart,
                    DateOfPlannedEnd = x.DateOfPlannedEnd,
                    DateOfComplete = x.DateOfComplete,
                    TaskStatus = x.TaskStatus
                })
                .ToListAsync();

            return tasks;
        }
    }
}
