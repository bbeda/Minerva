using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minerva.Application.Migrations
{
    /// <inheritdoc />
    public partial class TaskItemPlanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskItemPlanEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemPlanEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItemPlanEntries_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanEntries_TaskItemId",
                table: "TaskItemPlanEntries",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanEntries_TaskItemId_Type_Status",
                table: "TaskItemPlanEntries",
                columns: new[] { "TaskItemId", "Type", "Status" },
                unique: true,
                filter: """("Status" = 0)""");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanEntries_TenantId",
                table: "TaskItemPlanEntries",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemPlanEntries");
        }
    }
}
