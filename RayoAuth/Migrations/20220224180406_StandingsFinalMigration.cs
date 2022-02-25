using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RayoAuth.Migrations
{
    public partial class StandingsFinalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestProp",
                table: "Standings");

            migrationBuilder.AddColumn<long>(
                name: "CompetitionId",
                table: "Standings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Plan = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Standing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Stage = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    StandingsModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standing_Standings_StandingsModelId",
                        column: x => x.StandingsModelId,
                        principalTable: "Standings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CrestUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    PlayedGames = table.Column<long>(type: "bigint", nullable: false),
                    Won = table.Column<long>(type: "bigint", nullable: false),
                    Draw = table.Column<long>(type: "bigint", nullable: false),
                    Lost = table.Column<long>(type: "bigint", nullable: false),
                    Points = table.Column<long>(type: "bigint", nullable: false),
                    GoalsFor = table.Column<long>(type: "bigint", nullable: false),
                    GoalsAgainst = table.Column<long>(type: "bigint", nullable: false),
                    GoalDifference = table.Column<long>(type: "bigint", nullable: false),
                    StandingId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Table_Standing_StandingId",
                        column: x => x.StandingId,
                        principalTable: "Standing",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Table_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standings_CompetitionId",
                table: "Standings",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Standing_StandingsModelId",
                table: "Standing",
                column: "StandingsModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_StandingId",
                table: "Table",
                column: "StandingId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_TeamId",
                table: "Table",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_Competition_CompetitionId",
                table: "Standings",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_Competition_CompetitionId",
                table: "Standings");

            migrationBuilder.DropTable(
                name: "Competition");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "Standing");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Standings_CompetitionId",
                table: "Standings");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "Standings");

            migrationBuilder.AddColumn<string>(
                name: "TestProp",
                table: "Standings",
                type: "text",
                nullable: true);
        }
    }
}
