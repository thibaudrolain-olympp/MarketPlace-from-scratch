using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerProfileId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_User_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, "Chaussures", null },
                    { 2, "Sac / Hydratation", null },
                    { 3, "Accessoires", null },
                    { 4, "Combinaisons", null },
                    { 5, "Vélos", null },
                    { 6, "High-tech", null },
                    { 7, "Matériel trail", null },
                    { 8, "Nutrition", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Name", "Price", "Quantity", "SellerProfileId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(5746), "Chaussures trail pour terrains techniques et boueux.", "Chaussures de Trail Salomon Speedcross 5", 129.90m, 50, 1, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6234) },
                    { 2, 1, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6665), "Chaussures rapides pour triathlons et transitions rapides.", "Chaussures de Triathlon Asics Noosa Tri 15", 139.00m, 40, 1, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6666) },
                    { 3, 2, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6670), "Sac léger pour trail avec réservoir 1.5L.", "Sac d’hydratation Camelbak Ultra Pro Vest 7L", 119.99m, 25, 2, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6671) },
                    { 4, 3, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6673), "Ceinture légère et élastique pour dossard et gels.", "Ceinture porte-dossard triathlon Compressport", 19.90m, 100, 3, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6674) },
                    { 5, 4, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6677), "Néoprène pour natation en eau libre.", "Combinaison néoprène Orca Athlex Flow", 289.00m, 15, 2, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6678) },
                    { 6, 5, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6681), "Vélo performance route pour triathlons et compétitions.", "Vélo de route carbone Canyon Aeroad CF SLX", 3999.00m, 5, 4, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6682) },
                    { 7, 6, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6685), "Montre GPS multisport avec suivi performance trail/triathlon.", "Montre GPS Garmin Forerunner 965", 599.00m, 20, 1, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6685) },
                    { 8, 7, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6689), "Bâtons pliables ultralégers pour longues distances.", "Bâtons de trail Black Diamond Distance Carbon Z", 159.00m, 30, 3, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6689) },
                    { 9, 8, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6692), "Pack de gels énergétiques pour endurance.", "Pack gels énergétiques GU Energy (24x40g)", 38.00m, 200, 2, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6693) },
                    { 10, 3, new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6696), "Frontale haute performance pour trail nocturne.", "Lampe frontale Petzl Nao RL 1500 lumens", 159.90m, 25, 1, "active", new DateTime(2025, 8, 27, 14, 45, 28, 988, DateTimeKind.Utc).AddTicks(6696) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}