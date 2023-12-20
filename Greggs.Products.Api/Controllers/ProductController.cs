using System.Collections.Generic;
using Greggs.Products.Api.Constants;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    ///     Get list of products
    /// </summary>
    /// <param name="pageStart"></param>
    /// <param name="pageSize"></param>
    /// <returns>
    ///     Returns list of products
    /// </returns>
    /// <response code="200">Returns list of products</response>
    /// <response code="400">If pageStart or pageSize is invalid</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns list of products", typeof(ApiResponse<IEnumerable<Product>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If pageStart or pageSize is invalid", typeof(ApiResponse<string>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If there is an internal server error", typeof(ApiResponse<string>))]
    public ActionResult<ApiResponse<IEnumerable<Product>>> Get(int pageStart = 0, int pageSize = 5)
    {
        var products = _productService.GetProducts(pageStart, pageSize);
        return new ApiResponse<IEnumerable<Product>>
        {
            Data = products,
            HasErrors = false,
            Description = "Products retrieved successfully",
            Code = ApiResponseCodes.Success
        };
    }
}