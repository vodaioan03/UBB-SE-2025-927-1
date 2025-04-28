using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRoadmapTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roadmaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roadmaps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RoadmapId",
                table: "Sections",
                column: "RoadmapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Roadmaps_RoadmapId",
                table: "Sections",
                column: "RoadmapId",
                principalTable: "Roadmaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Roadmaps_RoadmapId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "Roadmaps");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RoadmapId",
                table: "Sections");
        }
    }
}
