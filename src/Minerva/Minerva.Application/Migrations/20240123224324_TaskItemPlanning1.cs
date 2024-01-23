using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minerva.Application.Migrations
{
    /// <inheritdoc />
    public partial class TaskItemPlanning1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskItemPlanEntries_TaskItemId_Type_Status",
                table: "TaskItemPlanEntries");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanEntries_TaskItemId_Type_Status",
                table: "TaskItemPlanEntries",
                columns: new[] { "TaskItemId", "Type", "Status" },
                unique: true,
                filter: "(\"Status\" = 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskItemPlanEntries_TaskItemId_Type_Status",
                table: "TaskItemPlanEntries");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanEntries_TaskItemId_Type_Status",
                table: "TaskItemPlanEntries",
                columns: new[] { "TaskItemId", "Type", "Status" },
                unique: true,
                filter: "(\"Status\" = 0)");
        }
    }
}
