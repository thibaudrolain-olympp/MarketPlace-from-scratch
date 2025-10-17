using Marketplace.Domain.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;

namespace Marketplace
{
    public class MarketplaceDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options)
        {
        }



        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<CartItem> CartItems => Set<CartItem>();

        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).UseIdentityColumn();
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).UseIdentityColumn();
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).UseIdentityColumn();
                entity.HasOne(o => o.Category).WithMany(p => p.Products);
                entity.HasMany(o => o.Images).WithOne(p => p.Product);

            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).UseIdentityColumn();
                entity.HasOne(o => o.Product).WithMany(c => c.Items);
               
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).UseIdentityColumn();
                entity.HasMany(o => o.Items).WithOne(c => c.Cart);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).UseIdentityColumn();
            });

            /*            Database.EnsureDeleted();
                        Database.Migrate();
                        SaveChanges();*/


            var ListCategory = new List<Category>
            {
                new Category { Id = 1, Name = "Chaussures" },
                new Category { Id = 2, Name = "Sac / Hydratation" },
                new Category { Id = 3, Name = "Accessoires" },
                new Category { Id = 4, Name = "Combinaisons" },
                new Category { Id = 5, Name = "Vélos" },
                new Category { Id = 6, Name = "High-tech" },
                new Category { Id = 7, Name = "Matériel trail" },
                new Category { Id = 8, Name = "Nutrition" }
            };
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(ListCategory);


            modelBuilder.Entity<Product>().HasData(
                     new
                     {
                         Id = 1,
                         SellerProfileId = 1,
                         Name = "Chaussures de Trail Salomon Speedcross 5",
                         Description = "Chaussures trail pour terrains techniques et boueux.",
                         Price = 129.90m,
                         Quantity = 50,
                         CategoryId = 1,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 2,
                         SellerProfileId = 1,
                         Name = "Chaussures de Triathlon Asics Noosa Tri 15",
                         Description = "Chaussures rapides pour triathlons et transitions rapides.",
                         Price = 139.00m,
                         Quantity = 40,
                         CategoryId = 1,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 3,
                         SellerProfileId = 2,
                         Name = "Sac d’hydratation Camelbak Ultra Pro Vest 7L",
                         Description = "Sac léger pour trail avec réservoir 1.5L.",
                         Price = 119.99m,
                         Quantity = 25,
                         CategoryId = 2,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 4,
                         SellerProfileId = 3,
                         Name = "Ceinture porte-dossard triathlon Compressport",
                         Description = "Ceinture légère et élastique pour dossard et gels.",
                         Price = 19.90m,
                         Quantity = 100,
                         CategoryId = 3,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 5,
                         SellerProfileId = 2,
                         Name = "Combinaison néoprène Orca Athlex Flow",
                         Description = "Néoprène pour natation en eau libre.",
                         Price = 289.00m,
                         Quantity = 15,
                         CategoryId = 4,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 6,
                         SellerProfileId = 4,
                         Name = "Vélo de route carbone Canyon Aeroad CF SLX",
                         Description = "Vélo performance route pour triathlons et compétitions.",
                         Price = 3999.00m,
                         Quantity = 5,
                         CategoryId = 5,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 7,
                         SellerProfileId = 1,
                         Name = "Montre GPS Garmin Forerunner 965",
                         Description = "Montre GPS multisport avec suivi performance trail/triathlon.",
                         Price = 599.00m,
                         Quantity = 20,
                         CategoryId = 6,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 8,
                         SellerProfileId = 3,
                         Name = "Bâtons de trail Black Diamond Distance Carbon Z",
                         Description = "Bâtons pliables ultralégers pour longues distances.",
                         Price = 159.00m,
                         Quantity = 30,
                         CategoryId = 7,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 9,
                         SellerProfileId = 2,
                         Name = "Pack gels énergétiques GU Energy (24x40g)",
                         Description = "Pack de gels énergétiques pour endurance.",
                         Price = 38.00m,
                         Quantity = 200,
                         CategoryId = 8,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     },
                     new
                     {
                         Id = 10,
                         SellerProfileId = 1,
                         Name = "Lampe frontale Petzl Nao RL 1500 lumens",
                         Description = "Frontale haute performance pour trail nocturne.",
                         Price = 159.90m,
                         Quantity = 25,
                         CategoryId = 3,
                         Status = "active",
                         CreatedAt = DateTime.UtcNow,
                         UpdatedAt = DateTime.UtcNow
                     }
                 );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
/*                warnings.Ignore(CoreEventId.AccidentalEntityType);*/

            }

                   );
        }
    }
}