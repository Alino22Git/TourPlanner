using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlannerDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTourTransportType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourLogs_TourLogId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourLogId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TourLogId",
                table: "Tours");

            migrationBuilder.AddColumn<string>(
                name: "TransportType",
                table: "Tours",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransportType",
                table: "Tours");

            migrationBuilder.AddColumn<int>(
                name: "TourLogId",
                table: "Tours",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourLogId",
                table: "Tours",
                column: "TourLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourLogs_TourLogId",
                table: "Tours",
                column: "TourLogId",
                principalTable: "TourLogs",
                principalColumn: "Id");
        }
    }
}
