using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_generator.Migrations
{
    /// <inheritdoc />
    public partial class epic_table_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Epics",
                columns: table => new
                {
                    EpicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProjectDomainId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epics", x => x.EpicId);
                    table.ForeignKey(
                        name: "FK_Epics_ProjectCategories_ProjectCategoryId",
                        column: x => x.ProjectCategoryId,
                        principalTable: "ProjectCategories",
                        principalColumn: "ProjectCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Epics_ProjectDomains_ProjectDomainId",
                        column: x => x.ProjectDomainId,
                        principalTable: "ProjectDomains",
                        principalColumn: "ProjectDomainId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpicTechStack",
                columns: table => new
                {
                    EpicId = table.Column<int>(type: "int", nullable: false),
                    TechStackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpicTechStack", x => new { x.EpicId, x.TechStackId });
                    table.ForeignKey(
                        name: "FK_EpicTechStack_Epics_EpicId",
                        column: x => x.EpicId,
                        principalTable: "Epics",
                        principalColumn: "EpicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpicTechStack_TechStacks_TechStackId",
                        column: x => x.TechStackId,
                        principalTable: "TechStacks",
                        principalColumn: "TechStackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Epics_ProjectCategoryId",
                table: "Epics",
                column: "ProjectCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Epics_ProjectDomainId",
                table: "Epics",
                column: "ProjectDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_EpicTechStack_TechStackId",
                table: "EpicTechStack",
                column: "TechStackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpicTechStack");

            migrationBuilder.DropTable(
                name: "Epics");
        }
    }
}
