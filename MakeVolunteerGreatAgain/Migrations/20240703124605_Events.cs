using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakeVolunteerGreatAgain.Migrations
{
    /// <inheritdoc />
    public partial class Events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AspNetUsers_CommonUserId1",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_AspNetUsers_CommonUserId1",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Volunteers_CommonUserId1",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CommonUserId1",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CommonUserId1",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "CommonUserId1",
                table: "Organizations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommonUserId1",
                table: "Volunteers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonUserId1",
                table: "Organizations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_CommonUserId1",
                table: "Volunteers",
                column: "CommonUserId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CommonUserId1",
                table: "Organizations",
                column: "CommonUserId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AspNetUsers_CommonUserId1",
                table: "Organizations",
                column: "CommonUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_AspNetUsers_CommonUserId1",
                table: "Volunteers",
                column: "CommonUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
