using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace nappeandcloe.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Damages_Orders_OrderId",
                table: "Damages");

            migrationBuilder.DropForeignKey(
                name: "FK_Losts_Orders_OrderId",
                table: "Losts");

            migrationBuilder.DropForeignKey(
                name: "FK_PickUps_Orders_OrderId",
                table: "PickUps");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Orders_OrderId",
                table: "Returns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Returns",
                table: "Returns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PickUps",
                table: "PickUps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Losts",
                table: "Losts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Damages",
                table: "Damages");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Returns",
                newName: "OrderDetailId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "PickUps",
                newName: "OrderDetailId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Losts",
                newName: "OrderDetailId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Damages",
                newName: "OrderDetailId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Returns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Returns",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "LinerId",
                table: "Returns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailOrderId",
                table: "Returns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailProductId",
                table: "Returns",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "PickUps",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PickUps",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "LinerId",
                table: "PickUps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailOrderId",
                table: "PickUps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailProductId",
                table: "PickUps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Losts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Losts",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "LinerId",
                table: "Losts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailOrderId",
                table: "Losts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailProductId",
                table: "Losts",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Damages",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Damages",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "LinerId",
                table: "Damages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailOrderId",
                table: "Damages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailProductId",
                table: "Damages",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Returns",
                table: "Returns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PickUps",
                table: "PickUps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Losts",
                table: "Losts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Damages",
                table: "Damages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_LinerId",
                table: "Returns",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_OrderDetailOrderId_OrderDetailProductId",
                table: "Returns",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_PickUps_LinerId",
                table: "PickUps",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_PickUps_OrderDetailOrderId_OrderDetailProductId",
                table: "PickUps",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Losts_LinerId",
                table: "Losts",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Losts_OrderDetailOrderId_OrderDetailProductId",
                table: "Losts",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Damages_LinerId",
                table: "Damages",
                column: "LinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_OrderDetailOrderId_OrderDetailProductId",
                table: "Damages",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Damages_Liners_LinerId",
                table: "Damages",
                column: "LinerId",
                principalTable: "Liners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Damages_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "Damages",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" },
                principalTable: "OrderDetails",
                principalColumns: new[] { "OrderId", "ProductId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Losts_Liners_LinerId",
                table: "Losts",
                column: "LinerId",
                principalTable: "Liners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Losts_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "Losts",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" },
                principalTable: "OrderDetails",
                principalColumns: new[] { "OrderId", "ProductId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickUps_Liners_LinerId",
                table: "PickUps",
                column: "LinerId",
                principalTable: "Liners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickUps_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "PickUps",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" },
                principalTable: "OrderDetails",
                principalColumns: new[] { "OrderId", "ProductId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Liners_LinerId",
                table: "Returns",
                column: "LinerId",
                principalTable: "Liners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "Returns",
                columns: new[] { "OrderDetailOrderId", "OrderDetailProductId" },
                principalTable: "OrderDetails",
                principalColumns: new[] { "OrderId", "ProductId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Damages_Liners_LinerId",
                table: "Damages");

            migrationBuilder.DropForeignKey(
                name: "FK_Damages_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "Damages");

            migrationBuilder.DropForeignKey(
                name: "FK_Losts_Liners_LinerId",
                table: "Losts");

            migrationBuilder.DropForeignKey(
                name: "FK_Losts_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "Losts");

            migrationBuilder.DropForeignKey(
                name: "FK_PickUps_Liners_LinerId",
                table: "PickUps");

            migrationBuilder.DropForeignKey(
                name: "FK_PickUps_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "PickUps");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Liners_LinerId",
                table: "Returns");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_OrderDetails_OrderDetailOrderId_OrderDetailProductId",
                table: "Returns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Returns",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_LinerId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_OrderDetailOrderId_OrderDetailProductId",
                table: "Returns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PickUps",
                table: "PickUps");

            migrationBuilder.DropIndex(
                name: "IX_PickUps_LinerId",
                table: "PickUps");

            migrationBuilder.DropIndex(
                name: "IX_PickUps_OrderDetailOrderId_OrderDetailProductId",
                table: "PickUps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Losts",
                table: "Losts");

            migrationBuilder.DropIndex(
                name: "IX_Losts_LinerId",
                table: "Losts");

            migrationBuilder.DropIndex(
                name: "IX_Losts_OrderDetailOrderId_OrderDetailProductId",
                table: "Losts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Damages",
                table: "Damages");

            migrationBuilder.DropIndex(
                name: "IX_Damages_LinerId",
                table: "Damages");

            migrationBuilder.DropIndex(
                name: "IX_Damages_OrderDetailOrderId_OrderDetailProductId",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "LinerId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "OrderDetailOrderId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "OrderDetailProductId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PickUps");

            migrationBuilder.DropColumn(
                name: "LinerId",
                table: "PickUps");

            migrationBuilder.DropColumn(
                name: "OrderDetailOrderId",
                table: "PickUps");

            migrationBuilder.DropColumn(
                name: "OrderDetailProductId",
                table: "PickUps");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Losts");

            migrationBuilder.DropColumn(
                name: "LinerId",
                table: "Losts");

            migrationBuilder.DropColumn(
                name: "OrderDetailOrderId",
                table: "Losts");

            migrationBuilder.DropColumn(
                name: "OrderDetailProductId",
                table: "Losts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "LinerId",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "OrderDetailOrderId",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "OrderDetailProductId",
                table: "Damages");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "Returns",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "PickUps",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "Losts",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "Damages",
                newName: "OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Returns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "PickUps",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Losts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Damages",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Returns",
                table: "Returns",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PickUps",
                table: "PickUps",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Losts",
                table: "Losts",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Damages",
                table: "Damages",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Damages_Orders_OrderId",
                table: "Damages",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Losts_Orders_OrderId",
                table: "Losts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickUps_Orders_OrderId",
                table: "PickUps",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Orders_OrderId",
                table: "Returns",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
