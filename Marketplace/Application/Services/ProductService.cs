using AutoMapper;
using Marketplace.Application.ServiceModels;
using Marketplace.Application.ServicesInterfaces;
using Marketplace.Domain.DataModels;
using Marketplace.Domain.Interfaces;

namespace Marketplace.Application.Services
{
    /// <summary>
    /// Service métier pour la gestion des produits.
    /// </summary>
    ///
    /// <inheritdoc/>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur avec injection du repository et du mapper.
        /// </summary>
        /// <param name="repository">Repository des produits</param>
        /// <param name="mapper">Mapper AutoMapper</param>
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<ProductServiceModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllProduitsAsync();
            var mapper = _mapper.Map<IList<ProductServiceModel>>(entities);

            foreach (var product in mapper)
            {
                foreach (var image in product.Images)
                {
                    byte[] tableau = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Assets/" + image.ImageUrl);
                    image.Image = Convert.ToBase64String(tableau);
                }
            }

            return mapper;
        }

        public async Task<ProductServiceModel?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<ProductServiceModel>(entity);
        }

        public async Task<ProductServiceModel> CreateAsync(ProductServiceModel dto)
        {
            var entity = _mapper.Map<Product>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ProductServiceModel>(created);
        }

        public async Task<ProductServiceModel?> UpdateAsync(int id, ProductServiceModel dto)
        {
            var entity = _mapper.Map<Product>(dto);
            entity.Id = id;
            var updated = await _repository.UpdateAsync(entity);
            return updated is null ? null : _mapper.Map<ProductServiceModel>(updated);
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}