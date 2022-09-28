using Greggs.Products.Api.Models;
using System.Collections.Generic;

namespace Greggs.Products.Api.Services;

public interface IProductService
{
    /// <summary>
    /// Gets a list of products.
    /// </summary>
    /// <param name="pageStart">Optional page number.</param>
    /// <param name="pageSize">Optional page size.</param>
    /// <param name="currencyISOCode">Optional currency ISO code. If provided, products will be returned with prices in specified currency.</param>
    /// <returns>A list of products, optionally with prices converted to specified currency (GBP by default).</returns>
    IEnumerable<Product> GetProducts(int? pageStart, int? pageSize, string? currencyISOCode = null);
}