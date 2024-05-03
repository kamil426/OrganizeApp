using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration) 
        {
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(connectionString)
               .EnableSensitiveDataLogging());

            return services;
        }
    }
}
