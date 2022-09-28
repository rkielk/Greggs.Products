using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace Greggs.Products.UnitTests.DataAccess
{
    [TestFixture]
    public class ExchangeRateAccessTests
    {
        private MockRepository mockRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private ExchangeRateAccess CreateExchangeRateAccess()
        {
            return new ExchangeRateAccess();
        }

        [Test]
        public void GetExchangeRate_IfCurrencyFromDoesNotExist_ThrowsException()
        {
            // Arrange
            var exchangeRateAccess = this.CreateExchangeRateAccess();
            string isoCurrencyCodeFrom = "PLN";
            string isoCurrencyCodeTo = Currencies.GBP.Code;
            DateTime? date = null;

            // Assert
            var ex = Assert.Throws<NotSupportedException>(() => exchangeRateAccess.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date));

            Assert.That(ex?.Message, Is.EqualTo("Currency does not exist."));
        }

        [Test]
        public void GetExchangeRate_IfCurrencyToDoesNotExist_ThrowsException()
        {
            // Arrange
            var exchangeRateAccess = this.CreateExchangeRateAccess();
            string isoCurrencyCodeFrom = Currencies.GBP.Code;
            string isoCurrencyCodeTo = "PLN";
            DateTime? date = null;

            // Assert
            var ex = Assert.Throws<NotSupportedException>(() => exchangeRateAccess.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date));

            Assert.That(ex?.Message, Is.EqualTo("Currency does not exist."));
        }

        [Test]
        public void GetExchangeRate_IfDateNull_ReturnsLatest()
        {
            // Arrange
            var exchangeRateAccess = this.CreateExchangeRateAccess();
            string isoCurrencyCodeFrom = Currencies.GBP.Code;
            string isoCurrencyCodeTo = Currencies.EUR.Code;
            DateTime? date = null;

            // Act
            var result = exchangeRateAccess.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result?.Date, new DateTime(2022, 9, 27));
        }
        [Test]
        public void GetExchangeRate_IfDateProvided_ReturnsForSpecifiedDate()
        {
            // Arrange
            var exchangeRateAccess = this.CreateExchangeRateAccess();
            string isoCurrencyCodeFrom = Currencies.GBP.Code;
            string isoCurrencyCodeTo = Currencies.EUR.Code;
            DateTime? date = new DateTime(2022, 9, 25);

            // Act
            var result = exchangeRateAccess.GetExchangeRate(
                isoCurrencyCodeFrom,
                isoCurrencyCodeTo,
                date);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result?.Date, date);
        }

        [Test]
        public void List_ReturnsExchangeRates()
        {
            // Arrange
            var exchangeRateAccess = this.CreateExchangeRateAccess();
            int? pageStart = null;
            int? pageSize = null;

            // Act
            var result = exchangeRateAccess.List(
                pageStart,
                pageSize);

            // Assert
            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void List_WhenPageSizeProvided_ReturnsCorrectNumberOfRates()
        {
            // Arrange
            var exchangeRateAccess = this.CreateExchangeRateAccess();
            int? pageStart = null;
            int? pageSize = 2;

            // Act
            var result = exchangeRateAccess.List(
                pageStart,
                pageSize);

            // Assert
            Assert.AreEqual(result.Count(), 2);
        }
    }
}
