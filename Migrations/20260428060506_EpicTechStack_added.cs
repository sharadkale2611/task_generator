using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_generator.Migrations
{
    /// <inheritdoc />
    public partial class EpicTechStack_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EpicTechStack_Epics_EpicId",
                table: "EpicTechStack");

            migrationBuilder.DropForeignKey(
                name: "FK_EpicTechStack_TechStacks_TechStackId",
                table: "EpicTechStack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EpicTechStack",
                table: "EpicTechStack");

            migrationBuilder.RenameTable(
                name: "EpicTechStack",
                newName: "EpicTechStacks");

            migrationBuilder.RenameIndex(
                name: "IX_EpicTechStack_TechStackId",
                table: "EpicTechStacks",
                newName: "IX_EpicTechStacks_TechStackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EpicTechStacks",
                table: "EpicTechStacks",
                columns: new[] { "EpicId", "TechStackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EpicTechStacks_Epics_EpicId",
                table: "EpicTechStacks",
                column: "EpicId",
                principalTable: "Epics",
                principalColumn: "EpicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EpicTechStacks_TechStacks_TechStackId",
                table: "EpicTechStacks",
                column: "TechStackId",
                principalTable: "TechStacks",
                principalColumn: "TechStackId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EpicTechStacks_Epics_EpicId",
                table: "EpicTechStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_EpicTechStacks_TechStacks_TechStackId",
                table: "EpicTechStacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EpicTechStacks",
                table: "EpicTechStacks");

            migrationBuilder.RenameTable(
                name: "EpicTechStacks",
                newName: "EpicTechStack");

            migrationBuilder.RenameIndex(
                name: "IX_EpicTechStacks_TechStackId",
                table: "EpicTechStack",
                newName: "IX_EpicTechStack_TechStackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EpicTechStack",
                table: "EpicTechStack",
                columns: new[] { "EpicId", "TechStackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EpicTechStack_Epics_EpicId",
                table: "EpicTechStack",
                column: "EpicId",
                principalTable: "Epics",
                principalColumn: "EpicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EpicTechStack_TechStacks_TechStackId",
                table: "EpicTechStack",
                column: "TechStackId",
                principalTable: "TechStacks",
                principalColumn: "TechStackId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
