using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryScheduler.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineRatio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Ratio",
                table: "Machines",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ratio",
                table: "Machines");
        }
    }
}
