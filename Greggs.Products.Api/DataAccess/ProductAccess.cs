using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.DataAccess;

/// <summary>
/// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
/// </summary>
public class ProductAccess : IDataAccess<Product>
{
    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>()
    {
        new() { Name = "Sausage Roll", Price = 1m, Currency = Currencies.GBP },
        new() { Name = "Vegan Sausage Roll", Price = 1.1m, Currency = Currencies.GBP },
        new() { Name = "Steak Bake", Price = 1.2m, Currency = Currencies.GBP },
        new() { Name = "Yum Yum", Price = 0.7m, Currency = Currencies.GBP },
        new() { Name = "Pink Jammie", Price = 0.5m, Currency = Currencies.GBP },
        new() { Name = "Mexican Baguette", Price = 2.1m, Currency = Currencies.GBP },
        new() { Name = "Bacon Sandwich", Price = 1.95m, Currency = Currencies.GBP },
        new() { Name = "Coca Cola", Price = 1.2m, Currency = Currencies.GBP }
    };

    public IEnumerable<Product> List(int? pageStart, int? pageSize)
    {
        var queryable = ProductDatabase.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }
}