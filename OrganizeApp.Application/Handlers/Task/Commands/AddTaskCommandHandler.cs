using MediatR;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Shared.Task.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Commands
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Entities.Task()
            {
                DateOfPlannedStart = request.DateOfPlannedStart,
                Description = request.Description,
                Title = request.Title,
                DateOfPlannedEnd = request.DateOfPlannedEnd,
                TaskStatus = request.TaskStatus,
                UserId = request.UserId,
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
