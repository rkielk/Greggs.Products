using System;

namespace Greggs.Products.Api.Models;

public class ExchangeRate
{
    public Currency CurrencyFrom { get; set; }
    public Currency CurrencyTo { get; set; }
    public decimal Rate { get; set; }
    public DateTime Date { get; set; }
}