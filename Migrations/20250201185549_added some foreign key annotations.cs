using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_ERP.Migrations
{
    /// <inheritdoc />
    public partial class addedsomeforeignkeyannotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_customers_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_suppliers_SupplierId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Order",
                newName: "supplierId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Order",
                newName: "orderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_SupplierId",
                table: "Order",
                newName: "IX_Order_supplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                newName: "IX_Order_orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_customers_orderId",
                table: "Order",
                column: "orderId",
                principalTable: "customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_suppliers_supplierId",
                table: "Order",
                column: "supplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_customers_orderId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_suppliers_supplierId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "supplierId",
                table: "Order",
                newName: "SupplierId");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "Order",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_supplierId",
                table: "Order",
                newName: "IX_Order_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_orderId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_customers_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_suppliers_SupplierId",
                table: "Order",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
