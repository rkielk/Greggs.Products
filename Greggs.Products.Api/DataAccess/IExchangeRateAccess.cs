using Greggs.Products.Api.Models;
using System;

namespace Greggs.Products.Api.DataAccess;

public interface IExchangeRateAccess: IDataAccess<ExchangeRate>
{
    ExchangeRate? GetExchangeRate(string isoCurrencyCodeFrom, string isoCurrencyCodeTo, DateTime? date = null);
}