namespace Marketplace.Repositories
{
    /// <summary>
    /// Interface générique pour les opérations CRUD sur les entités.
    /// </summary>
    /// <typeparam name="T">Type de l'entité (doit être une classe)</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Récupère toutes les entités de type T.
        /// </summary>
        /// <returns>Liste des entités</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Récupère une entité par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'entité</param>
        /// <returns>L'entité trouvée ou null</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute une nouvelle entité.
        /// </summary>
        /// <param name="entity">Entité à ajouter</param>
        /// <returns>Entité ajoutée</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Met à jour une entité existante.
        /// </summary>
        /// <param name="entity">Entité à mettre à jour</param>
        /// <returns>Entité mise à jour ou null si non trouvée</returns>
        Task<T?> UpdateAsync(T entity);

        /// <summary>
        /// Supprime une entité par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'entité à supprimer</param>
        /// <returns>True si la suppression a réussi, sinon false</returns>
        Task<bool> DeleteAsync(int id);
    }
}
