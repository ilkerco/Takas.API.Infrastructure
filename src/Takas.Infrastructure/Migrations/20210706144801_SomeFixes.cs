using Microsoft.EntityFrameworkCore.Migrations;

namespace Takas.Infrastructure.Migrations
{
    public partial class SomeFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Chats_ChatId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ChatName",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_FromId",
                table: "Chats",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ToId",
                table: "Chats",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_FromId",
                table: "Chats",
                column: "FromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_ToId",
                table: "Chats",
                column: "ToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_FromId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_ToId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_FromId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ToId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatName",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ToId",
                table: "Chats");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Chats_ChatId",
                table: "AspNetUsers",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
