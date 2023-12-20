using System.Collections.Generic;
using System.IO;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Customs;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Controllers;

public class TestProductController
{
    private readonly ProductController _controller;
    
    public TestProductController()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        var currencyService = new CurrencyService(configuration);
        var productAccess = new ProductAccess(Mock.Of<ILogger<ProductAccess>>());
        var productService = new ProductService(productAccess, currencyService);
        _controller = new ProductController(productService);
    }
    [Fact]
    public void TestGet()
    {
        // Act
        var result = _controller.Get();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiResponse<IEnumerable<Product>>>(result.Value);
        Assert.False(result.Value.HasErrors);
    }

    [Fact]
    public void Test_Get_With_Invalid_PageSize_Errors()
    {
        // Act
        var act = () => _controller.Get(0, 0);

        // Assert
        Assert.Throws<BadRequestException>(act);
        Assert.Throws<BadRequestException>(() => _controller.Get(0, 0)).Message.Contains("Invalid pageSize");
    }
    
    [Fact]
    public void Test_Get_With_Invalid_PageStart_Errors()
    {
        // Act
        var result = () => _controller.Get(-1, 5);

        // Assert
        Assert.Throws<BadRequestException>(result);
        Assert.Throws<BadRequestException>(() => _controller.Get(-1, 5)).Message.Contains("Invalid pageStart");
    }
}