using Intelligent_Document_Processing_and_Analysis_API.DbContexts;
using Intelligent_Document_Processing_and_Analysis_API.Repositories;
using Intelligent_Document_Processing_and_Analysis_API.Services;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

/*builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Configuration/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"Configuration/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();*/

var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File($"{Globals.LogsFolderName}/warning-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File($"{Globals.LogsFolderName}/error-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
    .WriteTo.File($"{Globals.LogsFolderName}/fatal-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.WithProperty("Context", "HttpRequest"))
        .WriteTo.File($"{Globals.LogsFolderName}/http-requests-.txt", rollingInterval: RollingInterval.Day));

if (AppSettingsConstants.ENABLE_VERBOSE_LOGS_TO_FILE)
    loggerConfig.WriteTo.File($"{Globals.LogsFolderName}/verbose-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose);

if (AppSettingsConstants.ENABLE_DEBUG_LOGS_TO_FILE)
    loggerConfig.WriteTo.File($"{Globals.LogsFolderName}/debug-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug);

if (AppSettingsConstants.ENABLE_INFORMATION_LOGS_TO_FILE)
    loggerConfig.WriteTo.File($"{Globals.LogsFolderName}/information-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);

Log.Logger = loggerConfig.CreateLogger();

// Enable Serilog's self logging to a file
SelfLog.Enable(TextWriter => File.AppendAllText($"{Globals.LogsFolderName}/serilog-selflog.txt", TextWriter + Environment.NewLine));

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
    builder.Services.AddDbContext<SQLiteDbContext>(options => options.UseSqlite(AppSettingsConstants.SQLITE_CONNECTION_STRING));
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddHttpClient(Globals.CompletionHttpClientName, client =>
    {
        client.BaseAddress = new($"http://{AppSettingsConstants.LLM_HTTP_API_IP}:{AppSettingsConstants.LLM_HTTP_API_PORT}");
        client.Timeout = TimeSpan.FromSeconds(AppSettingsConstants.COMPLETION_HTTP_CLIENT_TIMEOUT);
    });

    // Repositories
    builder.Services.AddScoped<ILlmInteractionRepository, LlmInteractionRepository>();

    // Services
    builder.Services.AddScoped<ILlmCompletionService, LlmCompletionService>();
    builder.Services.AddScoped<ITextExtractionService, TextExtractionService>();
    builder.Services.AddScoped<ISaveFileService, SaveFileService>();

    builder.Services.AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = int.MaxValue;
    });

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = int.MaxValue;
    });

    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = int.MaxValue;
        options.MultipartHeadersLengthLimit = int.MaxValue;
    });

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
    });

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