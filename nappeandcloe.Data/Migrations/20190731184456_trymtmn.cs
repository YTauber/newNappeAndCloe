using Microsoft.EntityFrameworkCore.Migrations;

namespace nappeandcloe.Data.Migrations
{
    public partial class trymtmn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductSizes");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "ProductSizes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeId",
                table: "ProductSizes",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                table: "ProductSizes",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_SizeId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "ProductSizes");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductSizes",
                nullable: true);
        }
    }
}
