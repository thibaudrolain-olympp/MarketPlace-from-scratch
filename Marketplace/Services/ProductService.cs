using Marketplace.DataModels;
using Marketplace.Repositories;
using Marketplace.ServiceModels;
using AutoMapper;

namespace Marketplace.Services
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

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<ProductDto>(entity);
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ProductDto>(created);
        }

        public async Task<ProductDto?> UpdateAsync(int id, ProductDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            entity.Id = id;
            var updated = await _repository.UpdateAsync(entity);
            return updated is null ? null : _mapper.Map<ProductDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
