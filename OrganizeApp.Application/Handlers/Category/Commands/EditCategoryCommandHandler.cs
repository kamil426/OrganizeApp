using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Task.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Category.Commands
{
    public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.SingleAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                throw new Exception("Not Found");

            category.Name = request.Name;
            category.Color = request.Color;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
