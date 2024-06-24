using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Serilog;
using Serilog.Debugging;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("LOGS/warning-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File("LOGS/error-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
    .WriteTo.File("LOGS/fatal-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.WithProperty("Context", "HttpRequest"))
        .WriteTo.File("LOGS/http-requests-.txt", rollingInterval: RollingInterval.Day));

if (AppSettingsConstants.ENABLE_VERBOSE_LOGS_TO_FILE)
    loggerConfig.WriteTo.File("LOGS/verbose-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose);

if (AppSettingsConstants.ENABLE_DEBUG_LOGS_TO_FILE)
    loggerConfig.WriteTo.File("LOGS/debug-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug);

if (AppSettingsConstants.ENABLE_INFORMATION_LOGS_TO_FILE)
    loggerConfig.WriteTo.File("LOGS/information-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);

Log.Logger = loggerConfig.CreateLogger();

// Enable Serilog's self logging to a file
SelfLog.Enable(TextWriter => File.AppendAllText("serilog-selflog.txt", TextWriter + Environment.NewLine));

builder.Host.UseSerilog(); // Use Serilog as the logging provider

try
{
    // Set the console title
    Console.Title = AppSettingsConstants.CONSOLE_TITLE;

    // Disable console quick edit mode
    if (AppSettingsConstants.DISABLE_CONSOLE_QUICK_EDIT_MODE)
        ConsoleHelper.DisableConsoleQuickEditMode();

    // Change the default HTTP API IP and port
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Listen(AppSettingsConstants.HTTP_API_IP, AppSettingsConstants.HTTP_API_PORT);
    });

    // Add services to the container.
    // sqlite, automapper, etc.

    // Our Services
    //

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (AppSettingsConstants.ENABLE_SWAGGER)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Use Serilog to log HTTP requests
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("Context", "HttpRequest");
        };
    }); // Keep this above app.UseHttpsRedirection()

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}