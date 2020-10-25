using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JedzenioPlanner.Api.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(nullable: false),
                    EntityVersion = table.Column<double>(nullable: false),
                    EventName = table.Column<string>(nullable: true),
                    EventPublished = table.Column<DateTime>(nullable: false),
                    EventDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => new { x.EntityId, x.EntityVersion });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
