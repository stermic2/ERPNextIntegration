using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPNextIntegration.Migrations
{
    public partial class errormessageaddedtofailedentitys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "errorContent",
                table: "FailedQboWebhooks",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "errorContent",
                table: "FailedQboWebhooks");
        }
    }
}
