using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coapplicantlogicupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCoApplicant",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "CoApplicantId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CoApplicantId",
                table: "Users",
                column: "CoApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CoApplicantId",
                table: "Users",
                column: "CoApplicantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CoApplicantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CoApplicantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CoApplicantId",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "HasCoApplicant",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
