using System;
using Microsoft.Extensions.Configuration;

namespace Greggs.Products.Api.Services;

public class CurrencyService: ICurrencyService
{
    private readonly IConfiguration _configuration;

    public CurrencyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public decimal Convert(decimal price, string baseCurrency, string targetCurrency)
    {
        var baseCurrencyRates = _configuration[$"Rates:{baseCurrency}:{targetCurrency}"];
        var rateIsDecimal = decimal.TryParse(baseCurrencyRates, out var rate);
        if (!rateIsDecimal) throw new Exception($"{baseCurrency} to {targetCurrency} rate not found");
        return Math.Round(price * rate, 2);
    }
}
