using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Constants;
using Greggs.Products.Api.Customs;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.DataAccess;

/// <summary>
/// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
/// </summary>
public class ProductAccess : IDataAccess<Product>
{
    private readonly ILogger<ProductAccess> _logger;

    public ProductAccess(ILogger<ProductAccess> logger)
    {
        _logger = logger;
    }
    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>
    {
        new() { Name = "Sausage Roll", PriceInPounds = 1m },
        new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
        new() { Name = "Steak Bake", PriceInPounds = 1.2m },
        new() { Name = "Yum Yum", PriceInPounds = 0.7m },
        new() { Name = "Pink Jammie", PriceInPounds = 0.5m },
        new() { Name = "Mexican Baguette", PriceInPounds = 2.1m },
        new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m },
        new() { Name = "Coca Cola", PriceInPounds = 1.2m }
    };

    public IEnumerable<Product> List(int? pageStart, int? pageSize)
    {
        if(pageStart < 0)
        {
            _logger.LogError("Invalid pageStart");
            throw new BadRequestException("Invalid pageStart");
        }
        if(pageSize <= 0)
        {
            _logger.LogError("Invalid pageSize");
            throw new BadRequestException("Invalid pageSize");
        }
        var queryable = ProductDatabase.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }
}