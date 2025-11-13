using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyJira.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class MemberIdComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MemberId",
                table: "Comments",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Members_MemberId",
                table: "Comments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Members_MemberId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MemberId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Comments");
        }
    }
}
