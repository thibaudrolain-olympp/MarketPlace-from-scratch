using Marketplace.DataModels;
using Marketplace.ServiceModels;

namespace Marketplace.Services
{
    /// <summary>
    /// Interface pour le service métier de gestion des produits.
    /// Définit les opérations CRUD sur les produits.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <returns>Liste des produits sous forme de ProductDto</returns>
        Task<IEnumerable<ProductDto>> GetAllAsync();

        /// <summary>
        /// Récupère un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du produit</param>
        /// <returns>ProductDto correspondant ou null si non trouvé</returns>
        Task<ProductDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crée un nouveau produit.
        /// </summary>
        /// <param name="product">Données du produit à créer</param>
        /// <returns>ProductDto du produit créé</returns>
        Task<ProductDto> CreateAsync(ProductDto product);

        /// <summary>
        /// Met à jour un produit existant.
        /// </summary>
        /// <param name="id">Identifiant du produit à mettre à jour</param>
        /// <param name="updated">Nouvelles données du produit</param>
        /// <returns>ProductDto mis à jour ou null si non trouvé</returns>
        Task<ProductDto?> UpdateAsync(int id, ProductDto updated);

        /// <summary>
        /// Supprime un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du produit à supprimer</param>
        /// <returns>True si la suppression a réussi, sinon false</returns>
        Task<bool> DeleteAsync(int id);
    }
}
