using System;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ILogger<CurrencyService> _logger;
    private readonly IExchangeRateAccess _exchangeRates;

    public CurrencyService(ILogger<CurrencyService> logger, IExchangeRateAccess exchangeRates)
    {
        _logger = logger;
        _exchangeRates = exchangeRates;
    }

    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <param name="isoCurrencyCodeFrom">The ISO Code of the currency to convert from.</param>
    /// <param name="isoCurrencyCodeTo">The ISO Code of the currency to convert to.</param>
    /// <param name="date">The date for which to retrieve the exchange rate for. If not provided, latest exchange rate will be retrieved.</param>
    /// <returns>The exchange rate between given currencies for the given date (or latest if date is not provided).</returns>
    public ExchangeRate GetExchangeRate(string isoCurrencyCodeFrom, string isoCurrencyCodeTo, DateTime? date = null)
    {
        if (isoCurrencyCodeFrom.ToUpper().Equals(isoCurrencyCodeTo.ToUpper()))
        {
            this._logger.LogWarning("Unable to get exchange rate where Currency From and Currency To are the same. Currency: {currency}.", isoCurrencyCodeFrom);
            throw new NotSupportedException($"Exchange rate does not exist.");
        }

        var exchangeRate = this._exchangeRates.GetExchangeRate(isoCurrencyCodeFrom, isoCurrencyCodeTo, date);

        if (exchangeRate == null)
        {
            this._logger.LogWarning("Exchange rate does not exist. Currency from: {currFrom}, currency to: {currTo}, date: {date}.", isoCurrencyCodeFrom, isoCurrencyCodeTo, date);
            throw new NotSupportedException($"Exchange rate does not exist.");
        }

        return exchangeRate;
    }

    /// <summary>
    /// Converts the currency.
    /// </summary>
    /// <param name="exchangeRate">The exchange rate to use for conversion.</param>
    /// <param name="amount">The amount to convert.</param>
    /// <returns>The converted amount.</returns>
    public decimal ConvertCurrency(ExchangeRate exchangeRate, decimal amount)
    {
        return exchangeRate.Rate * amount;
    }


}