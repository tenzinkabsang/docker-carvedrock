using Serilog;

var name = typeof(Program).Assembly.GetName().Name;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Assembly", name)
    .WriteTo.Console()
    .WriteTo.Seq("http://host.docker.internal:5341")
    .CreateLogger();


try
{
    Log.ForContext("Args", args).Information("Starting program...");

    Console.WriteLine("Hello, World! From the Console");

    Log.Information("Finished execution!");
}
catch (Exception ex)
{
    Log.Error(ex, "Exception happened");
}
finally
{
    Log.CloseAndFlush();
}
