using MediatR;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Task.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            _context.Tasks.Remove(new Domain.Entities.Task() { Id = request.Id });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
