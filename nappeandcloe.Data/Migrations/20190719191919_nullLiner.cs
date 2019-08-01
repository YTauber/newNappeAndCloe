using Microsoft.EntityFrameworkCore.Migrations;

namespace nappeandcloe.Data.Migrations
{
    public partial class nullLiner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Liners_LinerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_LinerId",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_LinerId",
                table: "Orders",
                column: "LinerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Liners_LinerId",
                table: "Orders",
                column: "LinerId",
                principalTable: "Liners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
