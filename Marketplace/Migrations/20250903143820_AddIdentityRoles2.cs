using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 427, DateTimeKind.Utc).AddTicks(9440), new DateTime(2025, 9, 3, 14, 31, 58, 427, DateTimeKind.Utc).AddTicks(9748) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(37), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(38) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(40), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(40) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(42), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(43) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(45), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(45) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(47), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(48) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(50), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(50) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(52), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(53) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(55), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(55) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(57), new DateTime(2025, 9, 3, 14, 31, 58, 428, DateTimeKind.Utc).AddTicks(57) });
        }
    }
}