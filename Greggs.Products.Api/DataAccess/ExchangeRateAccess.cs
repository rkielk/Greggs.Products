using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.DataAccess;

public class ExchangeRateAccess : IExchangeRateAccess
{
    private static readonly IEnumerable<ExchangeRate> ExchangeRateDatabase = new List<ExchangeRate>()
    {
        new() {
            CurrencyFrom = Currencies.GBP,
            CurrencyTo = Currencies.EUR,
            Date = new DateTime(2022, 9, 25), 
            Rate = 1.10m },
        new() {
            CurrencyFrom = Currencies.GBP,
            CurrencyTo = Currencies.EUR,
            Date = new DateTime(2022, 9, 27),
            Rate = 1.11m },
    };

    public ExchangeRate? GetExchangeRate(string isoCurrencyCodeFrom, string isoCurrencyCodeTo, DateTime? date = null)
    {
        var currencyFrom = GetCurrencyFromISO(isoCurrencyCodeFrom);
        var currencyTo = GetCurrencyFromISO(isoCurrencyCodeTo);

        var exchangeRates = ExchangeRateDatabase
            .AsQueryable()
            .Where(x => x.CurrencyFrom == currencyFrom && x.CurrencyTo == currencyTo);

        if (date != null)
        {
            exchangeRates = exchangeRates.Where(x => x.Date.Date.Equals(date.Value.Date));
        }

        return exchangeRates.OrderByDescending(x => x.Date).FirstOrDefault();
    }

    public IEnumerable<ExchangeRate> List(int? pageStart, int? pageSize)
    {
        var queryable = ExchangeRateDatabase.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }

    private static Currency GetCurrencyFromISO(string isoCode)
    {
        return isoCode switch
        {
            "GBP" => Currencies.GBP,
            "EUR" => Currencies.EUR,
            _ => throw new NotSupportedException("Currency does not exist.")
        };
    }
}