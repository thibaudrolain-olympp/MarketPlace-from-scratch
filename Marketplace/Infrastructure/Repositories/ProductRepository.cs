using Marketplace.Domain.DataModels;
using Marketplace.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories
{
    /// <summary>
    /// Repository spécifique pour la gestion des entités Product.
    /// Hérite du repository générique.
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        /// <summary>
        /// Constructeur avec injection du contexte de base de données.
        /// </summary>
        /// <param name="db">Contexte de base de données Marketplace</param>
        public ProductRepository(MarketplaceDbContext db) : base(db) { }

        public async Task<IList<Product>> GetAllProduitsAsync() => await _dbSet.AsNoTracking().Include(c => c.Images).ToListAsync();
    }
}