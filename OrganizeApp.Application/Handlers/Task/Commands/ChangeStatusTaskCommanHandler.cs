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
    public class ChangeStatusTaskCommanHandler : IRequestHandler<ChangeStatusTaskCommand>
    {

        private IApplicationDbContext _context;

        public ChangeStatusTaskCommanHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Handle(ChangeStatusTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.SingleAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (task == null)
                throw new Exception("Not Found");

            task.DateOfComplete = request.DateOfComplete;
            task.TaskStatus = request.TaskStatus;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
