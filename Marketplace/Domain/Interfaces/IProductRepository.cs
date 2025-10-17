using Marketplace.Domain.DataModels;

namespace Marketplace.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //Spécifique au produit
        Task<IList<Product>> GetAllProduitsAsync();
    }
}