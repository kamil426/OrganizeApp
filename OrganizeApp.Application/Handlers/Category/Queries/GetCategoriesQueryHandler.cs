using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Category.Dtos;
using OrganizeApp.Shared.Category.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Handlers.Category.Queries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context
                .Categories
                .Where(x => x.UserId == request.UserId)
                .AsNoTracking()
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                })
                .ToListAsync();

            return categories;
        }
    }
}
