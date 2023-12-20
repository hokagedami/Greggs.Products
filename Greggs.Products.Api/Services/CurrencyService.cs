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
        throw new NotImplementedException();
    }
}
