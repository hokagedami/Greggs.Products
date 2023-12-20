using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Extenstions;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog config to the builder
    builder.AddSerilogConfig(); // <--- This is the extension method

    // Add services to the container.
    var services = builder.Services;

    services.AddSingleton<IDataAccess<Product>, ProductAccess>();
    services.AddControllers();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Greggs.Products.Api", Version = "v1" });
    });


    // WebApplicationBuilder app
    var app = builder.Build();
    if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greggs Products Api V1");
        c.DocumentTitle = "Greggs Products API";
    });

    #region Middlewares
    // Middlewares here
    
    #endregion
    
    app.UseHttpsRedirection();
    app.AddCorsConfig();
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


    // Run the application.
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}