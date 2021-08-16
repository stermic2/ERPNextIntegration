using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPNextIntegration.Migrations
{
    public partial class id_to_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "SalesInvoices",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "FailedQboWebhooks",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SalesInvoices",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FailedQboWebhooks",
                newName: "id");
        }
    }
}
