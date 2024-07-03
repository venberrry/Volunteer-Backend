using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakeVolunteerGreatAgain.Migrations
{
    /// <inheritdoc />
    public partial class AddCommonUserIdToUsers3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_CommonUserId",
                table: "Organizations");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CommonUserId",
                table: "Organizations",
                column: "CommonUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_CommonUserId",
                table: "Organizations");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CommonUserId",
                table: "Organizations",
                column: "CommonUserId");
        }
    }
}
