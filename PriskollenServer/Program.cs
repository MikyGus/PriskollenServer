using PriskollenServer.Library.Services;
using PriskollenServer.Library.Services.StoreChains;
using PriskollenServer.Library.Services.Stores;
using PriskollenServer.Library.Validators;
using Serilog;

// Serilog config
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
#if DEBUG
    .AddJsonFile("appsettings.Development.json")
#endif
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application starting up");
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    {
        // Use serilog as the logger of choice
        builder.Host.UseSerilog();

        builder.Services.AddControllers();
        builder.Services.AddScoped<IDataAccess, DataAccess>();
        builder.Services.AddScoped<IStoreChainService, StoreChainService>();
        builder.Services.AddScoped<IStoreService, StoreService>();
        builder.Services.AddScoped<IStoreChainValidator, StoreChainValidator>();
        builder.Services.AddScoped<IGpsPositionValidator, GpsPositionValidator>();
        builder.Services.AddScoped<IStoreValidator, StoreValidator>();
    }

    WebApplication app = builder.Build();
    {
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
    Log.Information("Application stopped successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}