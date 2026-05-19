using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryScheduler.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceEmergencyWithPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmergency",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Jobs");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmergency",
                table: "Jobs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
