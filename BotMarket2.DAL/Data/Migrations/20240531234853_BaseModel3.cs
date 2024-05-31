using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotMarket2.Migrations
{
    /// <inheritdoc />
    public partial class BaseModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Algorithms_AspNetUsers_UserId",
                table: "Algorithms");

            migrationBuilder.DropForeignKey(
                name: "FK_Simulations_AspNetUsers_UserId",
                table: "Simulations");

            migrationBuilder.DropIndex(
                name: "IX_Simulations_UserId",
                table: "Simulations");

            migrationBuilder.DropIndex(
                name: "IX_Algorithms_UserId",
                table: "Algorithms");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Simulations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Algorithms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Simulations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Algorithms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Simulations_UserId",
                table: "Simulations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Algorithms_UserId",
                table: "Algorithms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Algorithms_AspNetUsers_UserId",
                table: "Algorithms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Simulations_AspNetUsers_UserId",
                table: "Simulations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
