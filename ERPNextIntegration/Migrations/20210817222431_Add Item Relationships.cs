using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPNextIntegration.Migrations
{
    public partial class AddItemRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemRelationships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRelationships", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemRelationships");
        }
    }
}
