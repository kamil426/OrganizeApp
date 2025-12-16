using MediatR;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Shared.Category.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Category.Commands
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Domain.Entities.Category()
            {
                Name = request.Name,
                Color = request.Color,
                UserId = request.UserId,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
