using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Greggs.Products.Api.Middlewares;

public class ContentTypeHandler
{
    private readonly RequestDelegate _next;

    public ContentTypeHandler(RequestDelegate next)
    {
        _next = next;
    }
    
    public Task Invoke(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        return _next(context);
    }
}