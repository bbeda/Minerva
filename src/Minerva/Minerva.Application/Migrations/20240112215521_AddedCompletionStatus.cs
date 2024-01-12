using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minerva.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompletionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedOn",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TaskItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TaskItems");
        }
    }
}
