using System.Collections.Generic;
using System.IO;
using System.Linq;
using Greggs.Products.Api.Customs;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Services;

public class TestProductService
{
    //Tests for products service
    private readonly ProductService _productService;
    
    public TestProductService()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        var currencyService = new CurrencyService(configuration);
        var productAccess = new ProductAccess(Mock.Of<ILogger<ProductAccess>>());
        _productService = new ProductService(productAccess, currencyService);
        
    }
    [Fact]
    public void Get_Products()
    {
        // Arrange
        const int pageStart = 0;
        const int pageSize = 2;
        var expectedProducts = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1m, PriceInEuro = 1.11m },
            new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m, PriceInEuro = 1.22m }
        };
        
        // Act
        var actualProducts = _productService.GetProducts(pageStart, pageSize).ToList();
        
        // Assert
        Assert.Equal(expectedProducts.Count, actualProducts.Count);
        for (var i = 0; i < expectedProducts.Count; i++)
        {
            Assert.Equal(expectedProducts[i].Name, actualProducts[i].Name);
            Assert.Equal(expectedProducts[i].PriceInPounds, actualProducts[i].PriceInPounds);
            Assert.Equal(expectedProducts[i].PriceInEuro, actualProducts[i].PriceInEuro);
        }
    }
    
    [Fact]
    public void Get_Products_Invalid_PageStart()
    {
        // Arrange
        const int pageStart = -1;
        const int pageSize = 2;
        
        // Act
        var exception = Assert.Throws<BadRequestException>(() => _productService.GetProducts(pageStart, pageSize));
        
        // Assert
        Assert.Equal("Invalid pageStart", exception.Message);
    }
    
    [Fact]
    public void Get_Products_Invalid_PageSize()
    {
        // Arrange
        const int pageStart = 0;
        const int pageSize = 0;
        
        // Act
        var exception = Assert.Throws<BadRequestException>(() => _productService.GetProducts(pageStart, pageSize));
        
        // Assert
        Assert.Equal("Invalid pageSize", exception.Message);
    }
}