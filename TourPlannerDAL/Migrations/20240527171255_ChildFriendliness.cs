using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlannerDAL.Migrations
{
    /// <inheritdoc />
    public partial class ChildFriendliness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildFriendliness",
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
                name: "ChildFriendliness",
                table: "Tours");
        }
    }
}
