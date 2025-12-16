using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Category.Queries;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Category.Queries
{
    public class GetToEditCategoryQueryHandler : IRequestHandler<GetToEditCategoryQuery, EditCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public GetToEditCategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditCategoryCommand> Handle(GetToEditCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.SingleAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                throw new Exception("Not Found");

            return new EditCategoryCommand()
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
            };
        }
    }
}
