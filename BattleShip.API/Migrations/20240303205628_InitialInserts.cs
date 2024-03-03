using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleShip.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialInserts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LeaderBoards",
                columns: new[] { "Id", "Name", "Score" },
                values: new object[,] {{ 1, "Julie", 260 }, { 2, "Eliot", 270}, {3, "Anonymous", 150}, {4, "LeBGdu69_lol", 60}}
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM LeaderBoards WHERE Id < 5", true);
        }
    }
}
