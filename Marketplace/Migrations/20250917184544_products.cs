using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");*/

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Name", "Price", "Quantity", "SellerProfileId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(3459), "Chaussures trail pour terrains techniques et boueux.", "Chaussures de Trail Salomon Speedcross 5", 129.90m, 50, 1, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(3463) },
                    { 2, 1, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5059), "Chaussures rapides pour triathlons et transitions rapides.", "Chaussures de Triathlon Asics Noosa Tri 15", 139.00m, 40, 1, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5059) },
                    { 3, 2, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5061), "Sac léger pour trail avec réservoir 1.5L.", "Sac d’hydratation Camelbak Ultra Pro Vest 7L", 119.99m, 25, 2, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5061) },
                    { 4, 3, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5063), "Ceinture légère et élastique pour dossard et gels.", "Ceinture porte-dossard triathlon Compressport", 19.90m, 100, 3, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5063) },
                    { 5, 4, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5064), "Néoprène pour natation en eau libre.", "Combinaison néoprène Orca Athlex Flow", 289.00m, 15, 2, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5065) },
                    { 6, 5, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5066), "Vélo performance route pour triathlons et compétitions.", "Vélo de route carbone Canyon Aeroad CF SLX", 3999.00m, 5, 4, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5067) },
                    { 7, 6, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5068), "Montre GPS multisport avec suivi performance trail/triathlon.", "Montre GPS Garmin Forerunner 965", 599.00m, 20, 1, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5069) },
                    { 8, 7, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5070), "Bâtons pliables ultralégers pour longues distances.", "Bâtons de trail Black Diamond Distance Carbon Z", 159.00m, 30, 3, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5070) },
                    { 9, 8, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5072), "Pack de gels énergétiques pour endurance.", "Pack gels énergétiques GU Energy (24x40g)", 38.00m, 200, 2, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5072) },
                    { 10, 9, new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5073), "Frontale haute performance pour trail nocturne.", "Lampe frontale Petzl Nao RL 1500 lumens", 159.90m, 25, 1, "active", new DateTime(2025, 9, 17, 18, 45, 42, 825, DateTimeKind.Utc).AddTicks(5074) }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");*/

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
