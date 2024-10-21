using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceXProductManagerApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComponentDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DependencyIds",
                table: "Components");

            migrationBuilder.CreateTable(
                name: "ComponentDependencies",
                columns: table => new
                {
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    DependencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDependencies", x => new { x.ComponentId, x.DependencyId });
                    table.ForeignKey(
                        name: "FK_ComponentDependencies_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentDependencies_Components_DependencyId",
                        column: x => x.DependencyId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDependencies_DependencyId",
                table: "ComponentDependencies",
                column: "DependencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentDependencies");

            migrationBuilder.AddColumn<string>(
                name: "DependencyIds",
                table: "Components",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
