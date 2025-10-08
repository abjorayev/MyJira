using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyJira.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedProjectBoardBunch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "TicketBoards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TicketBoards_ProjectId",
                table: "TicketBoards",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketBoards_Projects_ProjectId",
                table: "TicketBoards",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketBoards_Projects_ProjectId",
                table: "TicketBoards");

            migrationBuilder.DropIndex(
                name: "IX_TicketBoards_ProjectId",
                table: "TicketBoards");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TicketBoards");
        }
    }
}
