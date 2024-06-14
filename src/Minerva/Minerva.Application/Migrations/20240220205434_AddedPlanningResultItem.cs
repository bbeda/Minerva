using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minerva.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlanningResultItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskItemPlanningResultItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Result = table.Column<int>(type: "integer", nullable: false),
                    PlanningType = table.Column<int>(type: "integer", nullable: false),
                    PlanningDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemPlanningResultItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItemPlanningResultItems_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanningResultItems_TaskItemId",
                table: "TaskItemPlanningResultItems",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemPlanningResultItems_TenantId",
                table: "TaskItemPlanningResultItems",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemPlanningResultItems");
        }
    }
}
