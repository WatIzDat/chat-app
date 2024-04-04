using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameDateOfUnbanUtcToDateWillBeUnbannedUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_of_unban_utc",
                table: "ban",
                newName: "date_will_be_unbanned_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_will_be_unbanned_utc",
                table: "ban",
                newName: "date_of_unban_utc");
        }
    }
}
