using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizeApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Infrastructure.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.Property("Name")
                .HasMaxLength(40)
                .IsRequired();

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.HasMany(x => x.Tasks)
                .WithOne(x => x.Category);
        }
    }
}
