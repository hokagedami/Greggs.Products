using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Greggs.Products.Api.Customs;
using System.Text.Json;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Constants;

namespace Greggs.Products.Api.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandler> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    BadRequestException => ApiResponseCodes.BadRequest,
                    NotFoundException => ApiResponseCodes.NotFound,
                    _ => ApiResponseCodes.Error,
                };

                var errorResponse = new ApiResponse<string>
                {
                    HasErrors = true,
                    Description = error.Message,
                    Code = response.StatusCode,
                    Data = null
                };
                logger.LogError("Error occured::{Ex}", error);
               
                var result = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(result);
            }
        }
    }
}
