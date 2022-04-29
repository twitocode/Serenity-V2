using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Friendships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(16512709535580109L),
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897619L));

            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(16512709535579849L),
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897417L));

            migrationBuilder.AlterColumn<string>(
                name: "InstagramProfile",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<string>>(
                name: "FollowedTags",
                table: "AspNetUsers",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "DiscordProfile",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValue: "00000000-0000-0000-0000-000000000000")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendshipUser",
                columns: table => new
                {
                    FriendshipsId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendshipUser", x => new { x.FriendshipsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_FriendshipUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendshipUser_Friendships_FriendshipsId",
                        column: x => x.FriendshipsId,
                        principalTable: "Friendships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipUser_UsersId",
                table: "FriendshipUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendshipUser");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897619L),
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValue: NodaTime.Instant.FromUnixTimeTicks(16512709535580109L));

            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897417L),
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValue: NodaTime.Instant.FromUnixTimeTicks(16512709535579849L));

            migrationBuilder.AlterColumn<string>(
                name: "InstagramProfile",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "FollowedTags",
                table: "AspNetUsers",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscordProfile",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
