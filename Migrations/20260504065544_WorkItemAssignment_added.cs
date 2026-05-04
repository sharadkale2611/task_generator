using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_generator.Migrations
{
    /// <inheritdoc />
    public partial class WorkItemAssignment_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "WorkItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "WorkItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkItemAssignments",
                columns: table => new
                {
                    WorkItemAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkItemId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false),
                    AssignedByUserId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnassignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemAssignments", x => x.WorkItemAssignmentId);
                    table.ForeignKey(
                        name: "FK_WorkItemAssignments_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkItemAssignments_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItems",
                        principalColumn: "WorkItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_AssignedToUserId",
                table: "WorkItems",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemAssignments_AssignedToUserId",
                table: "WorkItemAssignments",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemAssignments_WorkItemId",
                table: "WorkItemAssignments",
                column: "WorkItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_AssignedToUserId",
                table: "WorkItems",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_AssignedToUserId",
                table: "WorkItems");

            migrationBuilder.DropTable(
                name: "WorkItemAssignments");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_AssignedToUserId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "WorkItems");
        }
    }
}
