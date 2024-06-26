using Microsoft.AspNetCore.Mvc;
using Serilog;
using ControllerWebAPI.Extensions;
[assembly: ApiController]


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers()
                    .AddNewtonsoftJson();

    builder.Services.AddLogging();

    builder.Services.RegisterApplicationServices(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
    }

    app.UseHttpsRedirection()
       .UseAuthentication()
       .UseAuthorization();

    app.MapControllers();

    await app.RunAsync();

    Log.Information("Application stopped without errors");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}