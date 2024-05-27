using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlannerDAL.Migrations
{
    /// <inheritdoc />
    public partial class Popularity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Tours",
                type: "integer",
                maxLength: 500,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Tours");
        }
    }
}
