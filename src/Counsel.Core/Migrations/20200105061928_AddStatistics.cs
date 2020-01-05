using Microsoft.EntityFrameworkCore.Migrations;

namespace Counsel.Core.Migrations
{
    public partial class AddStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Season = table.Column<int>(nullable: false),
                    Week = table.Column<int>(nullable: false),
                    PlayerId = table.Column<string>(nullable: false),
                    Points = table.Column<double>(nullable: false),
                    ProjectedPoints = table.Column<double>(nullable: false),
                    OpponentTeam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => new { x.Season, x.Week, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_Statistics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_PlayerId",
                table: "Statistics",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}
