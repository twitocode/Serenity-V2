using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreatedAtSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897619L),
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897417L),
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897619L));

            migrationBuilder.AlterColumn<Instant>(
                name: "CreationTime",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValue: NodaTime.Instant.FromUnixTimeTicks(16510935251897417L));
        }
    }
}
