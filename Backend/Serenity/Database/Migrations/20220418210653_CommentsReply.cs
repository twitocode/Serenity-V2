using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CommentsReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "RepliesToId",
                table: "Comments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RepliesToId",
                table: "Comments",
                column: "RepliesToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_RepliesToId",
                table: "Comments",
                column: "RepliesToId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_RepliesToId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_RepliesToId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "RepliesToId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Posts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Comments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "00000000-0000-0000-0000-000000000000");
        }
    }
}
