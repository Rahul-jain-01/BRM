using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class leveladded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "ReferralPaths",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "ReferralPaths");
        }
    }
}
