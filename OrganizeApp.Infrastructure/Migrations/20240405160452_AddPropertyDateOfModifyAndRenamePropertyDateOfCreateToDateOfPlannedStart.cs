using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyDateOfModifyAndRenamePropertyDateOfCreateToDateOfPlannedStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfCreate",
                table: "Tasks",
                newName: "DateOfPlannedStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfModify",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfModify",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DateOfPlannedStart",
                table: "Tasks",
                newName: "DateOfCreate");
        }
    }
}
