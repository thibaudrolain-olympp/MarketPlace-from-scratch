using AutoMapper;
using Marketplace.Controllers;
using Marketplace.Dto;
using Marketplace.ServiceModels;
using Marketplace.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Marketplace.Tests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CartController(_cartServiceMock.Object, _mapperMock.Object);
        }

        #region GetCart

        [Fact]
        public async Task GetCart_WhenCartExists_ReturnsOk()
        {
            // Arrange
            var userId = "user123";
            var cartModel = new CartServiceModel { UserId = userId };
            var cartDto = new CartDto { UserId = userId };

            _cartServiceMock.Setup(s => s.GetCartAsync(userId, default))
                .ReturnsAsync(cartModel);
            _mapperMock.Setup(m => m.Map<CartDto>(cartModel)).Returns(cartDto);

            // Act
            var result = await _controller.GetCart(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(userId, dto.UserId);
        }

        [Fact]
        public async Task GetCart_WhenServiceThrows_Returns500()
        {
            var userId = "user123";
            _cartServiceMock.Setup(s => s.GetCartAsync(userId, default))
                .ThrowsAsync(new Exception("DB error"));

            var result = await Record.ExceptionAsync(() => _controller.GetCart(userId));

            Assert.NotNull(result);
            Assert.IsType<Exception>(result);
        }

        #endregion GetCart

        #region AddItem

        [Fact]
        public async Task AddItem_WhenValid_ReturnsOk()
        {
            var userId = "user123";
            var request = new AddCartItemRequestDto { ProductId = 1, Quantity = 2, UnitPrice = 10 };
            var serviceModel = new CartItemServiceModel { ProductId = 1, Quantity = 2, UnitPrice = 10 };
            var updatedCart = new CartServiceModel { UserId = userId };
            var updatedCartDto = new CartDto { UserId = userId };

            _mapperMock.Setup(m => m.Map<CartItemServiceModel>(request)).Returns(serviceModel);
            _cartServiceMock.Setup(s => s.AddItemAsync(userId, serviceModel, default)).ReturnsAsync(updatedCart);
            _mapperMock.Setup(m => m.Map<CartDto>(updatedCart)).Returns(updatedCartDto);

            var result = await _controller.AddItem(userId, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(userId, dto.UserId);
        }

        [Fact]
        public async Task AddItem_WhenMapperReturnsNull_Throws()
        {
            var userId = "user123";
            var request = new AddCartItemRequestDto { ProductId = 1, Quantity = 2, UnitPrice = 10 };

            _mapperMock.Setup(m => m.Map<CartItemServiceModel>(request)).Returns((CartItemServiceModel)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.AddItem(userId, request));
        }

        #endregion AddItem

        #region UpdateItem

        [Fact]
        public async Task UpdateItem_WhenValid_ReturnsOk()
        {
            var userId = "user123";
            var productId = 1;
            var request = new UpdateCartItemRequestDto { Quantity = 5 };
            var updatedCart = new CartServiceModel { UserId = userId };
            var updatedDto = new CartDto { UserId = userId };

            _cartServiceMock.Setup(s => s.UpdateItemAsync(userId, productId, request.Quantity, default))
                .ReturnsAsync(updatedCart);
            _mapperMock.Setup(m => m.Map<CartDto>(updatedCart)).Returns(updatedDto);

            var result = await _controller.UpdateItem(userId, productId, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(userId, dto.UserId);
        }

        [Fact]
        public async Task UpdateItem_WhenCartNotFound_ReturnsOkWithEmptyCart()
        {
            var userId = "user123";
            var productId = 1;
            var request = new UpdateCartItemRequestDto { Quantity = 5 };

            _cartServiceMock.Setup(s => s.UpdateItemAsync(userId, productId, request.Quantity, default))
                .ReturnsAsync(new CartServiceModel { UserId = userId, Items = [] });
            _mapperMock.Setup(m => m.Map<CartDto>(It.IsAny<CartServiceModel>()))
                .Returns(new CartDto { UserId = userId, Items = [] });

            var result = await _controller.UpdateItem(userId, productId, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<CartDto>(okResult.Value);
            Assert.Empty(dto.Items);
        }

        #endregion UpdateItem

        #region RemoveItem

        [Fact]
        public async Task RemoveItem_WhenValid_ReturnsNoContent()
        {
            var userId = "user123";
            var productId = 1;

            _cartServiceMock.Setup(s => s.RemoveItemAsync(userId, productId, default))
                .Returns(Task.CompletedTask);

            var result = await _controller.RemoveItem(userId, productId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveItem_WhenServiceThrows_Propagates()
        {
            var userId = "user123";
            var productId = 1;

            _cartServiceMock.Setup(s => s.RemoveItemAsync(userId, productId, default))
                .ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<Exception>(() => _controller.RemoveItem(userId, productId));
        }

        #endregion RemoveItem

        #region Clear

        [Fact]
        public async Task Clear_WhenValid_ReturnsNoContent()
        {
            var userId = "user123";

            _cartServiceMock.Setup(s => s.ClearCartAsync(userId, default))
                .Returns(Task.CompletedTask);

            var result = await _controller.Clear(userId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Clear_WhenServiceThrows_Propagates()
        {
            var userId = "user123";
            _cartServiceMock.Setup(s => s.ClearCartAsync(userId, default))
                .ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<Exception>(() => _controller.Clear(userId));
        }

        #endregion Clear
    }
}