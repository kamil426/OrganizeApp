using Microsoft.EntityFrameworkCore;
using OrganizeApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<ApplicationUser> Users { get; }
        DbSet<Domain.Entities.Task> Tasks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
