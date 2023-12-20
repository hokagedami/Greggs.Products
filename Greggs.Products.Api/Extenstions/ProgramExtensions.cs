using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Greggs.Products.Api.Extenstions
{
    public static class ProgramExtensions
    {
        public static void AddSerilogConfig(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console();
            });
        }

        public static void AddCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                    .WithMethods("GET") // <-- Limit API to GET requests only
                    .WithHeaders("Content-Type", "accept");
            });
        }
    }
}
