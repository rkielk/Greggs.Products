using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Greggs.Products.UnitTests.Services
{
    [TestFixture]
    public class CurrencyServiceTests
    {
        private MockRepository _mockRepository;

        private Mock<ILogger<CurrencyService>> _mockLogger;
        private Mock<IExchangeRateAccess> _mockExchangeRateAccess;

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
            this._mockRepository = new MockRepository(MockBehavior.Strict);

            this._mockLogger = this._mockRepository.Create<ILogger<CurrencyService>>();
            this._mockExchangeRateAccess = this._mockRepository.Create<IExchangeRateAccess>();

            this._mockLogger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ));
        }

        private CurrencyService CreateService()
        {
            return new CurrencyService(
                this._mockLogger.Object,
                this._mockExchangeRateAccess.Object);
        }

        [Test]
        public void GetExchangeRate_WhenExists_ReturnsExchangeRate()
        {
            // Arrange
            var service = this.CreateService();
            string isoCurrencyCodeFrom = Currencies.GBP.Code;
            string isoCurrencyCodeTo = Currencies.EUR.Code;
            DateTime? date = null;
            this._mockExchangeRateAccess.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(exchangeRate);

            // Act
            var result = service.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetExchangeRate_WhenDoesNotExist_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            string isoCurrencyCodeFrom = Currencies.EUR.Code;
            string isoCurrencyCodeTo = Currencies.GBP.Code;
            DateTime? date = null;
            this._mockExchangeRateAccess.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>())).Returns((ExchangeRate)null!);

            // Assert
            var ex = Assert.Throws<NotSupportedException>(() => service.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date));

            Assert.That(ex?.Message, Is.EqualTo("Exchange rate does not exist."));
        }

        [Test]
        public void GetExchangeRate_WhenSameCurrencies_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            string isoCurrencyCodeFrom = Currencies.GBP.Code;
            string isoCurrencyCodeTo = Currencies.GBP.Code;
            DateTime? date = null;
            this._mockExchangeRateAccess.Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>())).Returns((ExchangeRate)null!);

            // Assert
            var ex = Assert.Throws<NotSupportedException>(() => service.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date));

            Assert.That(ex?.Message, Is.EqualTo("Exchange rate does not exist."));
        }

        [Test]
        public void GetExchangeRate_WhenCurrencyFromIsNull_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            string isoCurrencyCodeFrom = null!;
            string isoCurrencyCodeTo = Currencies.GBP.Code;
            DateTime? date = null;

            // Assert
            Assert.Throws<NullReferenceException>(() => service.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date));
        }

        [Test]
        public void GetExchangeRate_WhenCurrencyToIsNull_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            string isoCurrencyCodeFrom = Currencies.GBP.Code;
            string isoCurrencyCodeTo = null!;
            DateTime? date = null;

            // Assert
            Assert.Throws<NullReferenceException>(() => service.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date));
        }

        [Test]
        public void ConvertCurrency_WhenValid_ReturnsConvertedValue()
        {
            // Arrange
            var service = this.CreateService();
            decimal amount = 2;

            // Act
            var result = service.ConvertCurrency(
                exchangeRate,
                amount);

            // Assert
            Assert.AreEqual(result, 2.22m);
        }

        [Test]
        public void ConvertCurrency_WhenExchangeRateIsNull_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            decimal amount = 2;

            // Assert
            Assert.Throws<NullReferenceException>(() => service.ConvertCurrency(
                null!,
                amount));
        }
    }
}
