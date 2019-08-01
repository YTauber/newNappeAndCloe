using Microsoft.EntityFrameworkCore.Migrations;

namespace nappeandcloe.Data.Migrations
{
    public partial class taxexemt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "TaxExemt",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TaxExemt",
                table: "Customers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxExemt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TaxExemt",
                table: "Customers");

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
