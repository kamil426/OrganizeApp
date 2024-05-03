using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Commands
{
    public class EditTaskCommandHandler : IRequestHandler<EditTaskCommand>
    {
        private IApplicationDbContext _context;

        public EditTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Handle(EditTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (task == null)
                throw new Exception("Not Found");

            task.DateOfPlannedStart = request.DateOfPlannedStart;
            task.DateOfPlannedEnd = request.DateOfPlannedEnd;
            task.Description = request.Description;
            task.Title = request.Title;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
