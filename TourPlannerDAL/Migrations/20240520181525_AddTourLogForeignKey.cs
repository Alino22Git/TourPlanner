using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlannerDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTourLogForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourTourLog");

            migrationBuilder.AddColumn<int>(
                name: "TourLogId",
                table: "Tours",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TourId",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourLogId",
                table: "Tours",
                column: "TourLogId");

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourLogs_TourLogId",
                table: "Tours",
                column: "TourLogId",
                principalTable: "TourLogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourLogs_TourLogId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourLogId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "TourLogId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "TourLogs");

            migrationBuilder.CreateTable(
                name: "TourTourLog",
                columns: table => new
                {
                    SelectedToursId = table.Column<int>(type: "integer", nullable: false),
                    TourLogsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTourLog", x => new { x.SelectedToursId, x.TourLogsId });
                    table.ForeignKey(
                        name: "FK_TourTourLog_TourLogs_TourLogsId",
                        column: x => x.TourLogsId,
                        principalTable: "TourLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTourLog_Tours_SelectedToursId",
                        column: x => x.SelectedToursId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourTourLog_TourLogsId",
                table: "TourTourLog",
                column: "TourLogsId");
        }
    }
}
