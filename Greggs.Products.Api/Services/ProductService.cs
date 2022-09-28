using System.Collections.Generic;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IDataAccess<Product> _products;
    private readonly ICurrencyService _currencyService;

    public ProductService(ILogger<ProductService> logger, IDataAccess<Product> products, ICurrencyService currencyService)
    {
        _logger = logger;
        _products = products;
        _currencyService = currencyService;
    }

    /// <summary>
    /// Gets a list of products.
    /// </summary>
    /// <param name="pageStart">Optional page number.</param>
    /// <param name="pageSize">Optional page size.</param>
    /// <param name="currencyISOCode">Optional currency ISO code. If provided, products will be returned with prices in specified currency.</param>
    /// <returns>A list of products, optionally with prices converted to specified currency (GBP by default).</returns>
    public IEnumerable<Product> GetProducts(int? pageStart, int? pageSize, string? currencyISOCode = null)
    { 
        var products = _products.List(pageStart, pageSize);
        
        if (currencyISOCode == null || currencyISOCode == Currencies.GBP.Code)
        {
            return products;
        }

        return this.GetProductsWithConvertedCurrencies(products, currencyISOCode);
    }

    /// <summary>
    /// Convert the prices of a list of products to a specified currency.
    /// </summary>
    /// <param name="products">A list of products to convert.</param>
    /// <param name="currencyISOCode">Currency ISO code.</param>
    /// <returns>A list of products with prices converted to a specified currency.</returns>
    private IEnumerable<Product> GetProductsWithConvertedCurrencies(IEnumerable<Product> products, string currencyISOCode)
    {
        var exchangeRate = this._currencyService.GetExchangeRate(Currencies.GBP.Code, currencyISOCode);

        var convertedProducts = new List<Product>();

        foreach (var product in products)
        {
            convertedProducts.Add(
                product.CloneWithNewCurrency(
                    this._currencyService.ConvertCurrency(exchangeRate, product.Price),
                    exchangeRate.CurrencyTo));
        }

        return convertedProducts;
    }
}