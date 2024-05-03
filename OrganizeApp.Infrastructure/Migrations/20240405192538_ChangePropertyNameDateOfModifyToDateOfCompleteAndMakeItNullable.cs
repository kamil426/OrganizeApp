using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyNameDateOfModifyToDateOfCompleteAndMakeItNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfModify",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfComplete",
                table: "Tasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfComplete",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfModify",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
