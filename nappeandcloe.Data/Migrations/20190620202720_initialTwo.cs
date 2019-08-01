using Microsoft.EntityFrameworkCore.Migrations;

namespace nappeandcloe.Data.Migrations
{
    public partial class initialTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Damages_Product_ProductId",
                table: "Damages");

            migrationBuilder.DropForeignKey(
                name: "FK_Losts_Product_ProductId",
                table: "Losts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PickUps_Product_ProductId",
                table: "PickUps");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductLabels_Product_ProductId",
                table: "ProductLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Product_ProductId",
                table: "Returns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Damages_Products_ProductId",
                table: "Damages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Losts_Products_ProductId",
                table: "Losts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickUps_Products_ProductId",
                table: "PickUps",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLabels_Products_ProductId",
                table: "ProductLabels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Products_ProductId",
                table: "Returns",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Damages_Products_ProductId",
                table: "Damages");

            migrationBuilder.DropForeignKey(
                name: "FK_Losts_Products_ProductId",
                table: "Losts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PickUps_Products_ProductId",
                table: "PickUps");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductLabels_Products_ProductId",
                table: "ProductLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Products_ProductId",
                table: "Returns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Damages_Product_ProductId",
                table: "Damages",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Losts_Product_ProductId",
                table: "Losts",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickUps_Product_ProductId",
                table: "PickUps",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLabels_Product_ProductId",
                table: "ProductLabels",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Product_ProductId",
                table: "Returns",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
