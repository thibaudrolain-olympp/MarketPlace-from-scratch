using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddCartEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(8186), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(8611) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9021), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9022) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9025), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9025) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9028), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9029) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9032), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9032) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9035), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9036) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9039), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9039) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9042), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9043) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9045), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9046) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9049), new DateTime(2025, 9, 17, 14, 9, 27, 67, DateTimeKind.Utc).AddTicks(9049) });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2247), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2585) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2952), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2953) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2955), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2956) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2958), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2958) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2960), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2961) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2963), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2963) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2965), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2966) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2968), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(2969) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(3053), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(3053) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(3056), new DateTime(2025, 9, 3, 14, 38, 19, 533, DateTimeKind.Utc).AddTicks(3057) });
        }
    }
}