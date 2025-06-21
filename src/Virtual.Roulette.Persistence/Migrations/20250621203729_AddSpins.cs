using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Virtual.Roulette.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSpins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BetString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BetAmountCents = table.Column<long>(type: "bigint", nullable: false),
                    WonAmountCents = table.Column<long>(type: "bigint", nullable: false),
                    WinningNumber = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spins_UserId",
                table: "Spins",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spins");
        }
    }
}
