using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JedzenioPlanner.Api.Infrastructure.Migrations.RecipesDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Version = table.Column<double>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 3072, nullable: true),
                    PictureUrl = table.Column<string>(maxLength: 2048, nullable: true),
                    Calories = table.Column<int>(nullable: false),
                    Ingredients = table.Column<string>(nullable: true),
                    Steps = table.Column<string>(nullable: true),
                    MealTypes = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    Updates = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
