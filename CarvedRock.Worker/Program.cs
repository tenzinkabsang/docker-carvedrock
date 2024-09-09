using CarvedRock.Worker;
using Serilog;



var name = typeof(Program).Assembly.GetName().Name;

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
//    .Enrich.FromLogContext()
//    .Enrich.WithMachineName()
//    .Enrich.WithProperty("Assembly", name)
//    .WriteTo.Seq("http://host.docker.internal:5341")
//    .WriteTo.Console()
//    .CreateLogger();

try
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog(s => s
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", name)
            .WriteTo.Seq("http://host.docker.internal:5341")
            .WriteTo.Console()
            );

    builder.Services.AddHostedService<Worker>();

    IHost host = builder.Build();
    host.Run();
 }
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
