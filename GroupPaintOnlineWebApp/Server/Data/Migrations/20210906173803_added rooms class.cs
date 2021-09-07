using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupPaintOnlineWebApp.Server.Data.Migrations
{
    public partial class addedroomsclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RoomName = table.Column<string>(nullable: true),
                    CurrentUsers = table.Column<int>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Room");
        }
    }
}
