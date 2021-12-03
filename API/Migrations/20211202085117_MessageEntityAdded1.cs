using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class MessageEntityAdded1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_RecepientId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RecepientId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RecepientId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ResipientUsername",
                table: "Messages",
                newName: "RecipientUsername");

            migrationBuilder.RenameColumn(
                name: "RecepientDeleted",
                table: "Messages",
                newName: "RecipientDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_RecipientId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "RecipientUsername",
                table: "Messages",
                newName: "ResipientUsername");

            migrationBuilder.RenameColumn(
                name: "RecipientDeleted",
                table: "Messages",
                newName: "RecepientDeleted");

            migrationBuilder.AddColumn<int>(
                name: "RecepientId",
                table: "Messages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecepientId",
                table: "Messages",
                column: "RecepientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_RecepientId",
                table: "Messages",
                column: "RecepientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
