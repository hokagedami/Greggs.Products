using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System;
using Greggs.Products.Api.Models;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Linq;

namespace Greggs.Products.Api.Middlewares
{
    public class LoggerHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerHandler> _logger;

        public LoggerHandler(ILogger<LoggerHandler> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.Now;
            var model = new LogObject();

            model = await FormatRequest(context, model, _logger);

            model = await FormatResponse(context, model, _logger);

            var end = DateTime.Now;

            var duration = (end - start).TotalMilliseconds;

            model.Duration = duration.ToString(CultureInfo.InvariantCulture);

            _logger.LogInformation("{@Model}", model);
        }

        private static async Task<LogObject> FormatRequest(HttpContext context, LogObject model, ILogger logger)
        {
            try
            {
                var request = context.Request;
                model.Method = request.Method;
                model.Path = request.Path;
                model.HttpUri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
                model.RequestHeaders = string.Join("||", request.Headers.Select(x => $"{x.Key}: {x.Value}"));
                model.RequestParams = request.Query.ToString();
                var body = request.Body;
                request.EnableBuffering();
                request.Body.Position = 0;
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                _ = await request.Body.ReadAsync(buffer);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;
                model.RequestBody = bodyAsText;
                request.Body = body;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while formatting request");
            }
            return model;
        }
        private async Task<LogObject> FormatResponse(HttpContext context, LogObject model, ILogger logger)
        {
            var originalBody = context.Response.Body;
            try
            {
                using var memStream = new MemoryStream();
                context.Response.Body = memStream;

                await _next(context);

                memStream.Position = 0;
                var responseBody = await new StreamReader(memStream).ReadToEndAsync();

                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody);

                model.ResponseBody = responseBody;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while formatting response");
            }
            finally 
            {
                context.Response.Body = originalBody;
            }
            model.StatusCode = context.Response.StatusCode.ToString();
            model.ResponseHeaders = string.Join("||", context.Response.Headers.Select(x => $"{x.Key}: {x.Value}").ToArray());
            return model;
        }
    }
}
