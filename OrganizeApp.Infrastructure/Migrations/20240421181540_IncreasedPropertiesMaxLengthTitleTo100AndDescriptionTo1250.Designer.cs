﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrganizeApp.Infrastructure.Persistence;

#nullable disable

namespace OrganizeApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240421181540_IncreasedPrepetiesMaxLengthTitleTo100AndDescriptionTo1250")]
    partial class IncreasedPrepetiesMaxLengthTitleTo100AndDescriptionTo1250
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrganizeApp.Domain.Entities.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateOfComplete")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfPlannedEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfPlannedStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(1250)
                        .HasColumnType("nvarchar(1250)");

                    b.Property<int>("TaskStatus")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Tasks", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
