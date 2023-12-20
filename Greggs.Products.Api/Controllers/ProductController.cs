using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Constants;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<ApiResponse<IEnumerable<Product>>> Get(int pageStart = 0, int pageSize = 5)
    {
        var products = _productService.GetProducts(pageStart, pageSize);
        return Ok(new ApiResponse<IEnumerable<Product>>
        {
            Data = products,
            HasErrors = false,
            Description = "Products retrieved successfully",
            Code = ApiResponseCodes.Success
        });
    }
}