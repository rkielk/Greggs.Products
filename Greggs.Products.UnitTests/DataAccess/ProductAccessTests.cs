using Greggs.Products.Api.DataAccess;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace Greggs.Products.UnitTests.DataAccess
{
    [TestFixture]
    public class ProductAccessTests
    {
        private MockRepository mockRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private ProductAccess CreateProductAccess()
        {
            return new ProductAccess();
        }

        [Test]
        public void List_ReturnsProducts()
        {
            // Arrange
            var productAccess = this.CreateProductAccess();
            int? pageStart = null;
            int? pageSize = null;

            // Act
            var result = productAccess.List(
                pageStart,
                pageSize);

            // Assert
            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void List_WhenPageSizeProvided_ReturnsCorrectNumberOfProducts()
        {
            // Arrange
            var productAccess = this.CreateProductAccess();
            int? pageStart = null;
            int? pageSize = 2;

            // Act
            var result = productAccess.List(
                pageStart,
                pageSize);

            // Assert
            Assert.AreEqual(result.Count(), 2);
        }
    }
}
