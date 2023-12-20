namespace Greggs.Products.Api.Services;

public interface ICurrencyService
{
    decimal Convert(decimal price, string baseCurrency, string targetCurrency);
}