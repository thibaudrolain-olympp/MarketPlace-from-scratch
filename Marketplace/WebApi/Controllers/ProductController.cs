using AutoMapper;
using Marketplace.Application.Dto;
using Marketplace.Application.ServiceModels;
using Marketplace.Application.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.WebApi.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des produits.
    /// </summary>
    /// <remarks>
    /// Constructeur avec injection du service produit.
    /// </remarks>
    /// <param name="service">Service métier des produits</param>
    [ApiController]
    [Authorize]
    /*    [Authorize(Roles = "Admin")]
        [Authorize(Policy = "AdminOnly")]*/
    [Route("api/[controller]")]
    public class ProductController(IProductService _service, IMapper _mapper) : ControllerBase
    {
        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <returns>Liste des produits</returns>
        [HttpGet]
        public async Task<ActionResult<IList<ProductDto>>> GetAll()
        {
            try
            {
                var serviceModels = await _service.GetAllAsync();
                var dtoList = _mapper.Map<IList<ProductDto>>(serviceModels);
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }

        /// <summary>
        /// Récupère un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du produit</param>
        /// <returns>Produit ou non trouvé</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            try
            {
                var product = await _service.GetByIdAsync(id);
                if (product == null) return NotFound();
                return Ok(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }

        /// <summary>
        /// Crée un nouveau produit.
        /// </summary>
        /// <param name="product">Données du produit à créer</param>
        /// <returns>Produit créé</returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
        {
            if (product == null)
                return BadRequest();
            try
            {
                var created = await _service.CreateAsync(_mapper.Map<ProductServiceModel>(product));
                var dto = _mapper.Map<ProductDto>(created);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
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
            if (product == null)
                return BadRequest();
            try
            {
                var updated = await _service.UpdateAsync(id, _mapper.Map<ProductServiceModel>(product));
                if (updated == null) return NotFound();
                return Ok(_mapper.Map<ProductDto>(updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }

        /// <summary>
        /// Supprime un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du produit</param>
        /// <returns>NoContent ou NotFound</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }
    }
}