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
    public class GetTasksCheckListQueryHandler : IRequestHandler<GetTasksCheckListQuery, IEnumerable<TasksCheckListDto>>
    {
        private IApplicationDbContext _context;

        public GetTasksCheckListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        async Task<IEnumerable<TasksCheckListDto>> IRequestHandler<GetTasksCheckListQuery, IEnumerable<TasksCheckListDto>>.Handle(GetTasksCheckListQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _context
                .Tasks
                .Where(x => x.UserId == request.UserId)
                .AsNoTracking()
                .OrderBy(x => x.DateOfComplete)
                .Select(x => new TasksCheckListDto
                {
                    Title = x.Title,
                    TaskStatus = x.TaskStatus
                })
                .ToListAsync();

            return tasks;
        }
    }
}
