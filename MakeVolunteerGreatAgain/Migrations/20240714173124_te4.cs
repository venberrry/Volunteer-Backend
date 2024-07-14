using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakeVolunteerGreatAgain.Migrations
{
    /// <inheritdoc />
    public partial class te4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizations_OrganizationCommunUserId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "OrganizationCommunUserId",
                table: "Events",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizationCommunUserId",
                table: "Events",
                newName: "IX_Events_OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Events",
                newName: "OrganizationCommunUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizationId",
                table: "Events",
                newName: "IX_Events_OrganizationCommunUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizations_OrganizationCommunUserId",
                table: "Events",
                column: "OrganizationCommunUserId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
