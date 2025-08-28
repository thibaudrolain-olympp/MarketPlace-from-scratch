using global::Marketplace.Controllers;
using global::Marketplace.ServiceModels;
using global::Marketplace.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Marketplace.Tests
{                       
    // Classe de tests unitaires pour le contrôleur ProductController
    public class ProductControllerTests
    {
        // Mock du service utilisé par le contrôleur
        private readonly Mock<IProductService> _serviceMock;
        // Instance du contrôleur à tester
        private readonly ProductController _controller;

        // Constructeur : initialise le mock et le contrôleur
        public ProductControllerTests()
        {
            _serviceMock = new Mock<IProductService>();
            _controller = new ProductController(_serviceMock.Object);
        }

        // Test : Vérifie que GetAll retourne un OkObjectResult avec une liste de produits
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithProductList()
        {
            // Arrange : prépare une liste de produits fictive
            var products = new List<ProductDto> { new ProductDto(1, "Test", 10.0m) };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

            // Act : appelle la méthode du contrôleur
            var result = await _controller.GetAll();

            // Assert : vérifie le type de résultat et le contenu
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(products, okResult.Value);

            // Vérifie que le service a bien été appelé une fois
            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        // Test : Vérifie que GetAll retourne un OkObjectResult avec une liste vide
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithEmptyList()
        {
            // Arrange : prépare une liste vide
            var emptyList = new List<ProductDto>();
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAll();

            // Assert : le résultat doit être une liste vide
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Empty((IEnumerable<ProductDto>?)okResult.Value ?? []);

            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        // Test : Vérifie que GetAll retourne une erreur 500 si le service lève une exception
        [Fact]
        public async Task GetAll_WhenServiceThrows_ReturnsInternalServerError()
        {
            // Arrange : le service lève une exception
            _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _controller.GetAll();

            // Assert : le résultat doit être un ObjectResult avec le code 500
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        // Test : Vérifie que GetById retourne NotFound si le produit n'existe pas
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange : le service retourne null
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert : le résultat doit être NotFound
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test : Vérifie que Create retourne CreatedAtAction avec le produit créé
        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithProduct()
        {
            // Arrange : prépare un produit fictif
            var dto = new ProductDto(1, "Test", 10.0m);
            _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(dto);

            // Act
            var result = await _controller.Create(dto);

            // Assert : le résultat doit être CreatedAtActionResult avec le bon produit
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(dto, createdResult.Value);
        }
    }
}
