using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyJira.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class TicketIdToTaskLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "TaskLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TicketId",
                table: "TaskLogs",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Tickets_TicketId",
                table: "TaskLogs",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Tickets_TicketId",
                table: "TaskLogs");

            migrationBuilder.DropIndex(
                name: "IX_TaskLogs_TicketId",
                table: "TaskLogs");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TaskLogs");
        }
    }
}
