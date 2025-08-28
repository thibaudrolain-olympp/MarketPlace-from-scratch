using Marketplace.DataModels;
using Marketplace.ServiceModels;
using Marketplace.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des produits.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController: ControllerBase
    {
        // Service métier injecté pour la gestion des produits
        private readonly IProductService _service;

        /// <summary>
        /// Constructeur avec injection du service produit.
        /// </summary>
        /// <param name="service">Service métier des produits</param>
        public ProductController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <returns>Liste des produits</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Récupère un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du produit</param>
        /// <returns>Produit ou non trouvé</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        /// <summary>
        /// Crée un nouveau produit.
        /// </summary>
        /// <param name="product">Données du produit à créer</param>
        /// <returns>Produit créé</returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
        {
            var created = await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Met à jour un produit existant.
        /// </summary>
        /// <param name="id">Identifiant du produit</param>
        /// <param name="product">Données du produit à mettre à jour</param>
        /// <returns>Produit mis à jour ou non trouvé</returns>
        [HttpPut("{id:int}")]                                                                                       
        public async Task<ActionResult<ProductDto>> Update(int id, ProductDto product)
        {
            var updated = await _service.UpdateAsync(id, product);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Supprime un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du produit</param>
        /// <returns>NoContent ou NotFound</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
