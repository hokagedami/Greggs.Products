using System.Collections.Generic;
using System.IO;
using Greggs.Products.Api.Constants;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Services;

public class TestCurrencyService
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    [Fact]
    public void Convert_Price_To_USD()
    {
        // Arrange
        const decimal price = 1.00m;
        const string baseCurrency = CurrencyCodes.GBP;
        const string targetCurrency = CurrencyCodes.USD;
        const decimal expectedPrice = 1.32m;
        var currencyService = new CurrencyService(_configuration);
        
        // Act
        var actualPrice = currencyService.Convert(price, baseCurrency, targetCurrency);
        
        // Assert
        Assert.Equal(expectedPrice, actualPrice);
    }
    
    [Fact]
    public void Convert_Price_To_EUR()
    {
        // Arrange
        const decimal price = 1.00m;
        const string baseCurrency = CurrencyCodes.GBP;
        const string targetCurrency = CurrencyCodes.EUR;
        const decimal expectedPrice = 1.11m;
        var currencyService = new CurrencyService(_configuration);
        
        // Act
        var actualPrice = currencyService.Convert(price, baseCurrency, targetCurrency);
        
        // Assert
        Assert.Equal(expectedPrice, actualPrice);
    }
}