using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace nappeandcloe.Data.Migrations
{
    public partial class pickup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Damages");

            migrationBuilder.DropTable(
                name: "Losts");

            migrationBuilder.DropTable(
                name: "PickUps");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.AddColumn<int>(
                name: "Damages",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losts",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PickUps",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedNotUsed",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Returns",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Damages",
                table: "Liners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losts",
                table: "Liners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PickUps",
                table: "Liners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedNotUsed",
                table: "Liners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Returns",
                table: "Liners",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Damages",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Losts",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PickUps",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ReturnedNotUsed",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Returns",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Damages",
                table: "Liners");

            migrationBuilder.DropColumn(
                name: "Losts",
                table: "Liners");

            migrationBuilder.DropColumn(
                name: "PickUps",
                table: "Liners");

            migrationBuilder.DropColumn(
                name: "ReturnedNotUsed",
                table: "Liners");

            migrationBuilder.DropColumn(
                name: "Returns",
                table: "Liners");

            migrationBuilder.CreateTable(
                name: "Damages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Charge = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FixedAmount = table.Column<int>(nullable: false),
                    LinerId = table.Column<int>(nullable: true),
                    OrderDetailId = table.Column<int>(nullable: false),
                    OrderDetailOrderId = table.Column<int>(nullable: true),
                    OrderDetailProductId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Damages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Damages_Liners_LinerId",
                        column: x => x.LinerId,
                        principalTable: "Liners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Damages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Damages_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                        columns: x => new { x.OrderDetailOrderId, x.OrderDetailProductId },
                        principalTable: "OrderDetails",
                        principalColumns: new[] { "OrderId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Losts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Charge = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    LinerId = table.Column<int>(nullable: true),
                    LostAmount = table.Column<int>(nullable: false),
                    OrderDetailId = table.Column<int>(nullable: false),
                    OrderDetailOrderId = table.Column<int>(nullable: true),
                    OrderDetailProductId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Losts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Losts_Liners_LinerId",
                        column: x => x.LinerId,
                        principalTable: "Liners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Losts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Losts_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                        columns: x => new { x.OrderDetailOrderId, x.OrderDetailProductId },
                        principalTable: "OrderDetails",
                        principalColumns: new[] { "OrderId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PickUps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    LinerId = table.Column<int>(nullable: true),
                    OrderDetailId = table.Column<int>(nullable: false),
                    OrderDetailOrderId = table.Column<int>(nullable: true),
                    OrderDetailProductId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickUps_Liners_LinerId",
                        column: x => x.LinerId,
                        principalTable: "Liners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickUps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickUps_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                        columns: x => new { x.OrderDetailOrderId, x.OrderDetailProductId },
                        principalTable: "OrderDetails",
                        principalColumns: new[] { "OrderId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    LinerId = table.Column<int>(nullable: true),
                    OrderDetailId = table.Column<int>(nullable: false),
                    OrderDetailOrderId = table.Column<int>(nullable: true),
                    OrderDetailProductId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    UsedAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_Liners_LinerId",
                        column: x => x.LinerId,
                        principalTable: "Liners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Returns_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Returns_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                        columns: x => new { x.OrderDetailOrderId, x.OrderDetailProductId },
                        principalTable: "OrderDetails",
                        principalColumns: new[] { "OrderId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Damages_LinerId",
                table: "Damages",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_ProductId",
                table: "Damages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_OrderDetailOrderId_OrderDetailProductId",
                table: "Damages",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Losts_LinerId",
                table: "Losts",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Losts_ProductId",
                table: "Losts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Losts_OrderDetailOrderId_OrderDetailProductId",
                table: "Losts",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_PickUps_LinerId",
                table: "PickUps",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_PickUps_ProductId",
                table: "PickUps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PickUps_OrderDetailOrderId_OrderDetailProductId",
                table: "PickUps",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Returns_LinerId",
                table: "Returns",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ProductId",
                table: "Returns",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderDetailOrderId_OrderDetailProductId",
                table: "Returns",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });
        }
    }
}
