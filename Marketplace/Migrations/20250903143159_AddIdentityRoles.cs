using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(2159), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3080) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3817), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3819) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3824), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3825) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3829), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3830) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3835), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3836) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3840), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3842) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3846), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3847) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3852), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3853) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3857), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3858) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3863), new DateTime(2025, 8, 28, 18, 56, 20, 920, DateTimeKind.Utc).AddTicks(3864) });
        }
    }
}