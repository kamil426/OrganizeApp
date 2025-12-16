using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Category.Dtos;
using OrganizeApp.Shared.Task.Dtos;
using OrganizeApp.Shared.Task.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Queries
{
    public class GetTasksInculdingCategoryQueryHandler : IRequestHandler<GetTasksInculdingCategoryQuery, IEnumerable<TaskIncludingCategoryDto>>
    {
        private IApplicationDbContext _context;

        public GetTasksInculdingCategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskIncludingCategoryDto>> Handle(GetTasksInculdingCategoryQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _context
                .Tasks
                .Include(x => x.Category)
                .Where(x => x.UserId == request.UserId)
                .AsNoTracking()
                .OrderBy(x => x.DateOfComplete)
                .Select(x => new TaskIncludingCategoryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DateOfPlannedStart = x.DateOfPlannedStart,
                    DateOfPlannedEnd = x.DateOfPlannedEnd,
                    DateOfComplete = x.DateOfComplete,
                    TaskStatus = x.TaskStatus,
                    CategoryId = x.CategoryId,
                    CategoryDto = x.CategoryId == null ? null : new CategoryDto
                    {
                        Id = x.Category.Id,
                        Name = x.Category.Name,
                        Color = x.Category.Color
                    }
                })
                .ToListAsync();

            return tasks;
        }
    }
}
