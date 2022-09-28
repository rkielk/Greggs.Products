using Greggs.Products.Api.Models;
using System;

namespace Greggs.Products.Api.Services;

public interface ICurrencyService
{
    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <param name="isoCurrencyCodeFrom">The ISO Code of the currency to convert from.</param>
    /// <param name="isoCurrencyCodeTo">The ISO Code of the currency to convert to.</param>
    /// <param name="date">The date for which to retrieve the exchange rate for. If not provided, latest exchange rate will be retrieved.</param>
    /// <returns>The exchange rate between given currencies for the given date (or latest if date is not provided).</returns>
    ExchangeRate GetExchangeRate(string isoCurrencyCodeFrom, string isoCurrencyCodeTo, DateTime? date = null);

    /// <summary>
    /// Converts the currency.
    /// </summary>
    /// <param name="exchangeRate">The exchange rate to use for conversion.</param>
    /// <param name="amount">The amount to convert.</param>
    /// <returns>The converted amount.</returns>
    decimal ConvertCurrency(ExchangeRate exchangeRate, decimal amount);
}