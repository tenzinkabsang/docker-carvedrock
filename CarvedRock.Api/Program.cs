using CarvedRock.Api.Middlewares;
using CarvedRock.Api.Models;
using Serilog;
using Serilog.Events;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));


        // Wireup Dependency injection of my services
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, OrderService>();


        var app = builder.Build();

        app.UseMiddleware<CustomExceptionHandlingMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging(options =>
        {
            // health check exclusion
            // https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-excluding-health-check-endpoints-from-serilog
            options.GetLevel = ExcludeHealthChecks;
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
            };
        });

        ReadConfigValues(builder.Configuration);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception? ex) => ex != null
            ? LogEventLevel.Error
            : ctx.Response.StatusCode > 499
                ? LogEventLevel.Error
                : IsHealthCheckEndpoint(ctx) // Not an error, check if it was a health check
                    ? LogEventLevel.Verbose // Was a healthcheck, use Verbose
                    : LogEventLevel.Information;

    private static bool IsHealthCheckEndpoint(HttpContext ctx)
    {
        var userAgent = ctx.Request.Headers["User-Agent"].FirstOrDefault() ?? "";
        return ctx.Request.Path.Value.EndsWith("health", StringComparison.CurrentCultureIgnoreCase) || userAgent.Contains("HealthCheck", StringComparison.InvariantCultureIgnoreCase);
    }

    private static void ReadConfigValues(ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Db");
        var simpleProperty = configuration.GetValue<string>("SimpleProperty");
        var nestedProp = configuration.GetValue<string>("Inventory:NestedProperty");

        var dbgView = configuration.GetDebugView();
        Log.ForContext("ConfigurationDebug", dbgView).Information("Configuration dump.");

        //Log.ForContext("ConnectionString", connectionString)
        //    .ForContext("SimpleProperty", simpleProperty)
        //    .ForContext("Inventory:NestedProperty", nestedProp)
        //    .Information("Loaded configuration!", connectionString);
    }
}