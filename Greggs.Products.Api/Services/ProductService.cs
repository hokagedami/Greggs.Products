using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Constants;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services;

public class ProductService: IProductService
{
    private readonly IDataAccess<Product> _dataAccess;
    private readonly ICurrencyService _currencyService;

    public ProductService(IDataAccess<Product> dataAccess, ICurrencyService currencyService)
    {
        _dataAccess = dataAccess;
        _currencyService = currencyService;
    }

    public IEnumerable<Product> GetProducts(int? pageStart, int? pageSize)
    {
        return _dataAccess.List(pageStart, pageSize)
            .Select(pr => new Product
            {
                Name = pr.Name,
                PriceInPounds = pr.PriceInPounds,
                PriceInEuro = _currencyService.Convert(pr.PriceInPounds,
                    CurrencyCodes.GBP, CurrencyCodes.EUR)
            }).AsEnumerable();
    }
}