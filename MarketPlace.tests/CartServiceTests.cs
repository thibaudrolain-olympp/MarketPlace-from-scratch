using Xunit;
using Moq;
using Marketplace.DataModels;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Marketplace.Application.Services;
using Marketplace.Domain.DataModels;
using Marketplace.Domain.Interfaces;
using Marketplace.Application.ServiceModels;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> _cartRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly CartService _service;

    public CartServiceTests()
    {
        _service = new CartService(_cartRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetCartAsync_ReturnsNull_WhenCartNotFound()
    {
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync((Cart)null);
        var result = await _service.GetCartAsync("user", CancellationToken.None);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCartAsync_ReturnsCart_WhenFound()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem>() };
        var cartModel = new CartServiceModel { Id = 1, UserId = "user", Items = new List<CartItemServiceModel>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartServiceModel>(cart)).Returns(cartModel);
        var result = await _service.GetCartAsync("user", CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AddItemAsync_AddsNewItem_WhenNotExists()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem>() };
        var itemModel = new CartItemServiceModel { ProductId = 2, Quantity = 1, UnitPrice = 10 };
        var cartModel = new CartServiceModel { Id = 1, UserId = "user", Items = new List<CartItemServiceModel>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartItem>(itemModel)).Returns(new CartItem { Product = new Product { Id = 2 }, Quantity = 1, UnitPrice = 10 });
        _mapperMock.Setup(m => m.Map<CartServiceModel>(cart)).Returns(cartModel);
        var result = await _service.AddItemAsync("user", itemModel);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateItemAsync_UpdatesQuantity_WhenItemExists()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem> { new CartItem { Product = new Product { Id = 2 }, Quantity = 1 } } };
        var cartModel = new CartServiceModel { Id = 1, UserId = "user", Items = new List<CartItemServiceModel>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartServiceModel>(cart)).Returns(cartModel);
        var result = await _service.UpdateItemAsync("user", 2, 5);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task RemoveItemAsync_RemovesItem_WhenExists()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem> { new CartItem { Product = new Product { Id = 2 }, Quantity = 1 } } };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _cartRepoMock.Setup(r => r.RemoveItemAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        await _service.RemoveItemAsync("user", 2);
        _cartRepoMock.Verify(r => r.RemoveItemAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ClearCartAsync_ClearsItems()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem> { new CartItem { Product = new Product { Id = 2 }, Quantity = 1 } } };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _cartRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        await _service.ClearCartAsync("user");
        Assert.Empty(cart.Items);
    }

    [Fact]
    public async Task AddItemAsync_IncrementsQuantity_WhenItemAlreadyExists()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem> { new CartItem { Product = new Product { Id = 2 }, Quantity = 1, UnitPrice = 10 } } };
        var itemModel = new CartItemServiceModel { ProductId = 2, Quantity = 2, UnitPrice = 10 };
        var cartModel = new CartServiceModel { Id = 1, UserId = "user", Items = new List<CartItemServiceModel>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartItem>(itemModel)).Returns(new CartItem { Product = new Product { Id = 2 }, Quantity = 2, UnitPrice = 10 });
        _mapperMock.Setup(m => m.Map<CartServiceModel>(cart)).Returns(cartModel);
        var result = await _service.AddItemAsync("user", itemModel);
        Assert.Equal(3, cart.Items.First().Quantity);
    }

    [Fact]
    public async Task UpdateItemAsync_RemovesItem_WhenQuantityIsZeroOrNegative()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem> { new CartItem { Product = new Product { Id = 2 }, Quantity = 1 } } };
        var cartModel = new CartServiceModel { Id = 1, UserId = "user", Items = new List<CartItemServiceModel>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartServiceModel>(cart)).Returns(cartModel);
        var result = await _service.UpdateItemAsync("user", 2, 0);
        Assert.Empty(cart.Items);
    }

    [Fact]
    public async Task RemoveItemAsync_DoesNothing_WhenItemNotFound()
    {
        var cart = new Cart { Id = 1, UserId = "user", Items = new List<CartItem>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        await _service.RemoveItemAsync("user", 99);
        _cartRepoMock.Verify(r => r.RemoveItemAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddItemAsync_CreatesCart_WhenCartDoesNotExist()
    {
        Cart? nullCart = null;
        var newCart = new Cart { Id = 2, UserId = "user", Items = new List<CartItem>() };
        var itemModel = new CartItemServiceModel { ProductId = 3, Quantity = 1, UnitPrice = 20 };
        var cartModel = new CartServiceModel { Id = 2, UserId = "user", Items = new List<CartItemServiceModel>() };
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ReturnsAsync(nullCart);
        _cartRepoMock.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>())).ReturnsAsync(newCart);
        _mapperMock.Setup(m => m.Map<CartItem>(itemModel)).Returns(new CartItem { Product = new Product { Id = 3 }, Quantity = 1, UnitPrice = 20 });
        _mapperMock.Setup(m => m.Map<CartServiceModel>(newCart)).Returns(cartModel);
        var result = await _service.AddItemAsync("user", itemModel);
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
    }

    [Fact]
    public async Task GetCartAsync_ThrowsException_WhenRepositoryThrows()
    {
        _cartRepoMock.Setup(r => r.GetByUserIdAsync("user", It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception("Repo error"));
        await Assert.ThrowsAsync<System.Exception>(() => _service.GetCartAsync("user", CancellationToken.None));
    }
}
