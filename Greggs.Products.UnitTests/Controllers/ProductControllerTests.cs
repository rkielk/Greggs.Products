using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Greggs.Products.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private MockRepository _mockRepository;

        private Mock<ILogger<ProductController>> _mockLogger;
        private Mock<IProductService> _mockProductService;

        private static readonly Product product1 = new()
        {
            Name = "Test product 1",
            Currency = Currencies.GBP,
            Price = 1.11m
        };

        private static readonly Product product2 = new()
        {
            Name = "Test product 2",
            Currency = Currencies.GBP,
            Price = 1.5m
        };

        private static readonly Product product3 = new()
        {
            Name = "Test product 3",
            Currency = Currencies.GBP,
            Price = 1.95m
        };

        private static readonly Product product1EUR = new()
        {
            Name = "Test product 1",
            Currency = Currencies.EUR,
            Price = 1.2321m
        };

        private static readonly Product product2EUR = new()
        {
            Name = "Test product 2",
            Currency = Currencies.EUR,
            Price = 1.665m
        };

        private static readonly Product product3EUR = new()
        {
            Name = "Test product 3",
            Currency = Currencies.EUR,
            Price = 2.1645m
        };

        [SetUp]
        public void SetUp()
        {
            this._mockRepository = new MockRepository(MockBehavior.Strict);

            this._mockLogger = this._mockRepository.Create<ILogger<ProductController>>();
            this._mockProductService = this._mockRepository.Create<IProductService>();
        }

        private ProductController CreateProductController()
        {
            return new ProductController(
                this._mockLogger.Object,
                this._mockProductService.Object);
        }

        [Test]
        public void Get_WhenCurrencyNull_ReturnsProductsInGBP()
        {
            // Arrange
            var productController = this.CreateProductController();
            int pageStart = 0;
            int pageSize = 0;
            string currencyISO = null!;
            this._mockProductService
                .Setup(x => x.GetProducts(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>()))
                .Returns(new List<Product>() { product1, product2, product3 });

            // Act
            var result = productController.Get(
                pageStart,
                pageSize,
                currencyISO);

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(result.First().Currency.Code, Currencies.GBP.Code);
        }

        [Test]
        public void Get_WhenCurrencyNull_CallsProductsServiceWithGBPCode()
        {
            // Arrange
            var productController = this.CreateProductController();
            int pageStart = 0;
            int pageSize = 0;
            string currencyISO = null!;
            this._mockProductService
                .Setup(x => x.GetProducts(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>()))
                .Returns(new List<Product>() { product1, product2, product3 });

            // Act
            var result = productController.Get(
                pageStart,
                pageSize,
                currencyISO);

            // Assert
            this._mockProductService.Verify(x => x.GetProducts(pageStart, pageSize, Currencies.GBP.Code), Times.Once);
        }

        [Test]
        public void Get_WhenCurrencyProvided_CallsProductsServiceWithSpecifiedCurrency()
        {
            // Arrange
            var productController = this.CreateProductController();
            int pageStart = 0;
            int pageSize = 0;
            string currencyISO = Currencies.EUR.Code;
            this._mockProductService
                .Setup(x => x.GetProducts(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>()))
                .Returns(new List<Product>() { product1EUR, product2EUR, product3EUR });

            // Act
            var result = productController.Get(
                pageStart,
                pageSize,
                currencyISO);

            // Assert
            this._mockProductService.Verify(x => x.GetProducts(pageStart, pageSize, currencyISO), Times.Once);
        }

        [Test]
        public void Get_WhenPageStartAndSizeProvided_CallsProductsServiceWithCorrectParams()
        {
            // Arrange
            var productController = this.CreateProductController();
            int pageStart = 1;
            int pageSize = 2;
            string currencyISO = null!;
            this._mockProductService
                .Setup(x => x.GetProducts(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>()))
                .Returns(new List<Product>() { product1, product2, product3 });

            // Act
            var result = productController.Get(
                pageStart,
                pageSize,
                currencyISO);

            // Assert
            this._mockProductService.Verify(x => x.GetProducts(pageStart, pageSize, Currencies.GBP.Code), Times.Once);
        }
    }
}
