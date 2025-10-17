using Xunit;
using Moq;
using Marketplace.Services;
using Marketplace.Repositories;
using Marketplace.ServiceModels;
using Marketplace.DataModels;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _service = new ProductService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedProducts()
    {
        var products = new List<Product> { new Product { Id = 1, Name = "Test" } };
        var models = new List<ProductServiceModel> { new ProductServiceModel { Id = 1, Name = "Test" } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        _mapperMock.Setup(m => m.Map<IList<ProductServiceModel>>(products)).Returns(models);
        var result = await _service.GetAllAsync();
        Assert.Single(result);
        Assert.Equal("Test", result[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product)null);
        var result = await _service.GetByIdAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedProduct_WhenFound()
    {
        var product = new Product { Id = 1, Name = "Test" };
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductServiceModel>(product)).Returns(model);
        var result = await _service.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateAsync_ReturnsMappedCreatedProduct()
    {
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        var product = new Product { Id = 1, Name = "Test" };
        _mapperMock.Setup(m => m.Map<Product>(model)).Returns(product);
        _repoMock.Setup(r => r.AddAsync(product)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductServiceModel>(product)).Returns(model);
        var result = await _service.CreateAsync(model);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        var product = new Product { Id = 1, Name = "Test" };
        _mapperMock.Setup(m => m.Map<Product>(model)).Returns(product);
        _repoMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync((Product)null);
        var result = await _service.UpdateAsync(1, model);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsMappedProduct_WhenFound()
    {
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        var product = new Product { Id = 1, Name = "Test" };
        _mapperMock.Setup(m => m.Map<Product>(model)).Returns(product);
        _repoMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductServiceModel>(product)).Returns(model);
        var result = await _service.UpdateAsync(1, model);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _service.DeleteAsync(1);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotDeleted()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);
        var result = await _service.DeleteAsync(1);
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsException_WhenRepositoryFails()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new System.Exception("Repo error"));
        await Assert.ThrowsAsync<System.Exception>(() => _service.GetAllAsync());
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenRepositoryFails()
    {
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        var product = new Product { Id = 1, Name = "Test" };
        _mapperMock.Setup(m => m.Map<Product>(model)).Returns(product);
        _repoMock.Setup(r => r.AddAsync(product)).ThrowsAsync(new System.Exception("Repo error"));
        await Assert.ThrowsAsync<System.Exception>(() => _service.CreateAsync(model));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenRepositoryFails()
    {
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        var product = new Product { Id = 1, Name = "Test" };
        _mapperMock.Setup(m => m.Map<Product>(model)).Returns(product);
        _repoMock.Setup(r => r.UpdateAsync(product)).ThrowsAsync(new System.Exception("Repo error"));
        await Assert.ThrowsAsync<System.Exception>(() => _service.UpdateAsync(1, model));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenRepositoryFails()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new System.Exception("Repo error"));
        await Assert.ThrowsAsync<System.Exception>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public async Task CreateAsync_MapsNullProduct_ReturnsNull()
    {
        var model = new ProductServiceModel { Id = 1, Name = "Test" };
        _mapperMock.Setup(m => m.Map<Product>(model)).Returns((Product)null);
        _repoMock.Setup(r => r.AddAsync((Product)null)).ReturnsAsync((Product)null);
        var result = await _service.CreateAsync(model);
        Assert.Null(result);
    }
}
