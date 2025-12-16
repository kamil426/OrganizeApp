using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrganizeApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = OrganizeApp.Domain.Entities.Task;

namespace OrganizeApp.Infrastructure.Persistence.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("Tasks");

            builder.Property(x => x.Title)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1250);

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
