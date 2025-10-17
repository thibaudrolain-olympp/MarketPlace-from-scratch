using AutoMapper;
using global::Marketplace.Controllers;
using global::Marketplace.Services;
using Marketplace.Dto;
using Marketplace.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Marketplace.Tests
{
    // Classe de tests unitaires pour le contrôleur ProductController
    public class ProductControllerTests
    {
        // Mock du service utilisé par le contrôleur
        private readonly Mock<IProductService> _serviceMock;

        // Mock du mapper utilisé par le contrôleur
        private readonly Mock<IMapper> _mapperMock;

        // Instance du contrôleur à tester
        private readonly ProductController _controller;

        // Constructeur : initialise les mocks et le contrôleur
        public ProductControllerTests()
        {
            _serviceMock = new Mock<IProductService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProductController(_serviceMock.Object, _mapperMock.Object);
        }

        // Test : Vérifie que GetAll retourne un OkObjectResult avec une liste de ProductDto
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithProductList()
        {
            // Arrange : prépare une liste de ProductServiceModel et le mapping vers ProductDto
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModels = new List<ProductServiceModel>
            {
                new(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>())
            };
            var dtoList = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Desc",
                    Price = 10.0m,
                    Quantity = 1,
                    Status = "Active",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(serviceModels);
            _mapperMock.Setup(m => m.Map<IList<ProductDto>>(serviceModels)).Returns(dtoList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dtoList, okResult.Value);
            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        // Test : Vérifie que GetAll retourne un OkObjectResult avec une liste vide
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var serviceModels = new List<ProductServiceModel>();
            var dtoList = new List<ProductDto>();
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(serviceModels);
            _mapperMock.Setup(m => m.Map<IList<ProductDto>>(serviceModels)).Returns(dtoList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Empty((IEnumerable<ProductDto>?)okResult.Value ?? []);
            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        // Test : Vérifie que GetAll retourne une erreur 500 si le service lève une exception
        [Fact]
        public async Task GetAll_WhenServiceThrows_ReturnsInternalServerError()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        // Test : Vérifie que GetById retourne NotFound si le produit n'existe pas
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((ProductServiceModel?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test : Vérifie que GetById retourne OkObjectResult avec le produit demandé
        [Fact]
        public async Task GetById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModel = new ProductServiceModel(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>());
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 10.0m,
                Quantity = 1,
                Status = "Active",
                CreatedAt = serviceModel.CreatedAt,
                UpdatedAt = serviceModel.UpdatedAt
            };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(serviceModel);
            _mapperMock.Setup(m => m.Map<ProductDto>(serviceModel)).Returns(dto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        // Test : Vérifie que Create retourne CreatedAtAction avec le produit créé
        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithProduct()
        {
            // Arrange
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 10.0m,
                Quantity = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModel = new ProductServiceModel(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>());
            _mapperMock.Setup(m => m.Map<ProductServiceModel>(dto)).Returns(serviceModel);
            _serviceMock.Setup(s => s.CreateAsync(serviceModel)).ReturnsAsync(serviceModel);
            _mapperMock.Setup(m => m.Map<ProductDto>(serviceModel)).Returns(dto);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(dto, createdResult.Value);
        }

        // Test : Vérifie que Create retourne BadRequest si le produit est null
        [Fact]
        public async Task Create_ReturnsBadRequest_WhenProductIsNull()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestResult>(result.Result);
        }

        // Test : Vérifie que Create retourne InternalServerError si le service lève une exception
        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenServiceThrows()
        {
            // Arrange
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 10.0m,
                Quantity = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModel = new ProductServiceModel(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>());
            _mapperMock.Setup(m => m.Map<ProductServiceModel>(dto)).Returns(serviceModel);
            _serviceMock.Setup(s => s.CreateAsync(serviceModel)).ThrowsAsync(new Exception("Erreur"));

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, errorResult.StatusCode);
        }

        // Test : Vérifie que Update retourne BadRequest si le produit est null
        [Fact]
        public async Task Update_ReturnsBadRequest_WhenProductIsNull()
        {
            // Act
            var result = await _controller.Update(1, null);

            // Assert
            var badRequest = Assert.IsType<BadRequestResult>(result.Result);
        }

        // Test : Vérifie que Update retourne NotFound si le produit n'existe pas
        [Fact]
        public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 10.0m,
                Quantity = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModel = new ProductServiceModel(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>());
            _mapperMock.Setup(m => m.Map<ProductServiceModel>(dto)).Returns(serviceModel);
            _serviceMock.Setup(s => s.UpdateAsync(1, serviceModel)).ReturnsAsync((ProductServiceModel?)null);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test : Vérifie que Update retourne OkObjectResult avec le produit mis à jour
        [Fact]
        public async Task Update_ReturnsOkResult_WhenProductIsUpdated()
        {
            // Arrange
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Updated",
                Description = "UpdatedDesc",
                Price = 20.0m,
                Quantity = 2,
                Status = "Inactive",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModel = new ProductServiceModel(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>());
            _mapperMock.Setup(m => m.Map<ProductServiceModel>(dto)).Returns(serviceModel);
            _serviceMock.Setup(s => s.UpdateAsync(1, serviceModel)).ReturnsAsync(serviceModel);
            _mapperMock.Setup(m => m.Map<ProductDto>(serviceModel)).Returns(dto);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        // Test : Vérifie que Update retourne InternalServerError si le service lève une exception
        [Fact]
        public async Task Update_ReturnsInternalServerError_WhenServiceThrows()
        {
            // Arrange
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 10.0m,
                Quantity = 1,
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var category = new CategoryServiceModel { Id = 1, Name = "Cat", ParentId = null, Products = new List<ProductServiceModel>() };
            var serviceModel = new ProductServiceModel(1, "Test", "Desc", 10.0m, 1, category, "Active", DateTime.Now, DateTime.Now, new List<ProductImageServiceModel>());
            _mapperMock.Setup(m => m.Map<ProductServiceModel>(dto)).Returns(serviceModel);
            _serviceMock.Setup(s => s.UpdateAsync(1, serviceModel)).ThrowsAsync(new Exception("Erreur"));

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, errorResult.StatusCode);
        }

        // Test : Vérifie que Delete retourne NotFound si le produit n'existe pas
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test : Vérifie que Delete retourne NoContent si la suppression réussit
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenProductIsDeleted()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test : Vérifie que Delete retourne InternalServerError si le service lève une exception
        [Fact]
        public async Task Delete_ReturnsInternalServerError_WhenServiceThrows()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new Exception("Erreur"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
        }
    }
}