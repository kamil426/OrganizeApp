using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Category.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Category.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToDelete = _context.Categories.Single(x => x.Id == request.Id && x.UserId == request.UserId);
            var tasksWithDeletedCategories = _context.Tasks.Where(x => x.CategoryId == request.Id);

            await tasksWithDeletedCategories.ForEachAsync(x => x.CategoryId = null);

            _context.Categories.Remove(categoryToDelete);

            await _context.SaveChangesAsync();
        }
    }
}
