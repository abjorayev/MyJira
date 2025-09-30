using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyJira.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class ForgotToDoOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketBoardId",
                table: "Tickets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketBoardId",
                table: "Tickets",
                column: "TicketBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketBoards_TicketBoardId",
                table: "Tickets",
                column: "TicketBoardId",
                principalTable: "TicketBoards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketBoards_TicketBoardId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketBoardId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketBoardId",
                table: "Tickets");
        }
    }
}
