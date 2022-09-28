namespace Greggs.Products.Api.Models;

public class Currencies
{
    public static readonly Currency GBP = new() { Code = "GBP", Name = "British Pound" };
    public static readonly Currency EUR = new() { Code = "EUR", Name = "Euro" };
}