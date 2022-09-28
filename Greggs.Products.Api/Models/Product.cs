namespace Greggs.Products.Api.Models;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }

    internal Product CloneWithNewCurrency(decimal price, Currency currency)
    {
        return new()
        {
            Name = this.Name,
            Price = price,
            Currency = currency
        };
    }
}