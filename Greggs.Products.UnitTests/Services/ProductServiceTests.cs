using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Greggs.Products.UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<ProductService>> _mockLogger;
        private Mock<IDataAccess<Product>> _mockDataAccess;
        private Mock<ICurrencyService> _mockCurrencyService;

        private static readonly Product product1 = new() { 
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

        private static readonly Product product2EUR = new()
        {
            Name = "Test product 2",
            Currency = Currencies.EUR,
            Price = 1.665m
        };

        private static readonly ExchangeRate exchangeRate = new()
        {
            CurrencyFrom = Currencies.GBP,
            CurrencyTo = Currencies.EUR,
            Date = new DateTime(2022, 9, 27),
            Rate = 1.11m
        };

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this._mockLogger = this.mockRepository.Create<ILogger<ProductService>>();
            this._mockDataAccess = this.mockRepository.Create<IDataAccess<Product>>();
            this._mockCurrencyService = this.mockRepository.Create<ICurrencyService>();

            this._mockLogger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ));
        }

        private ProductService CreateService()
        {
            return new ProductService(
                this._mockLogger.Object,
                this._mockDataAccess.Object,
                this._mockCurrencyService.Object);
        }

        [Test]
        public void GetProducts_NullCurrency_ReturnsProducts()
        {
            // Arrange
            var service = this.CreateService();
            int? pageStart = null;
            int? pageSize = null;
            string currencyISOCode = null!;

            this._mockDataAccess
                .Setup(x => x.List(It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(new List<Product>() { product1, product2, product3 });

            // Act
            var result = service.GetProducts(
                pageStart,
                pageSize,
                currencyISOCode);

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void GetProducts_CurrencyProvided_ReturnsProductsInSpecifiedCurrency()
        {
            // Arrange
            var service = this.CreateService();
            int? pageStart = null;
            int? pageSize = null;
            string currencyISOCode = Currencies.EUR.Code;

            this._mockDataAccess
                .Setup(x => x.List(It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(new List<Product>() { product2 });

            this._mockCurrencyService
                .Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
                .Returns(exchangeRate);

            this._mockCurrencyService
                .Setup(x => x.ConvertCurrency(It.IsAny<ExchangeRate>(), It.IsAny<decimal>()))
                .Returns(product2EUR.Price);

            // Act
            var result = service.GetProducts(
                pageStart,
                pageSize,
                currencyISOCode);

            // Assert
            Assert.AreEqual(result.First().Currency.Code, currencyISOCode);
            Assert.AreEqual(result.First().Price, product2EUR.Price);
            Assert.AreEqual(result.First().Name, product2.Name);
        }
    }
}
