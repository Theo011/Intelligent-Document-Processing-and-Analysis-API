using Serilog;
using System.Net;

namespace Intelligent_Document_Processing_and_Analysis_API.Utilities;

public static class AppSettingsConstants
{
    public readonly static string CONSOLE_TITLE = null!;
    public readonly static string SQLITE_CONNECTION_STRING = null!;
    public readonly static string LLM_HTTP_API_IP = null!;
    public readonly static string LLM_HTTP_API_PORT = null!;
    public readonly static string LLM_COMPLETION_ENDPOINT = null!;
    public readonly static IPAddress HTTP_API_IP = null!;
    public readonly static int HTTP_API_PORT;
    public readonly static int COMPLETION_HTTP_CLIENT_TIMEOUT;
    public readonly static bool DISABLE_CONSOLE_QUICK_EDIT_MODE;
    public readonly static bool ENABLE_VERBOSE_LOGS_TO_FILE;
    public readonly static bool ENABLE_DEBUG_LOGS_TO_FILE;
    public readonly static bool ENABLE_INFORMATION_LOGS_TO_FILE;
    public readonly static bool ENABLE_SWAGGER;

    static AppSettingsConstants()
    {
        try
        {
            /*var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json")
                .Build();*/

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // CONSOLE_TITLE
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:CONSOLE_TITLE"]))
                CONSOLE_TITLE = configuration["AppSettings:CONSOLE_TITLE"]!;
            else
                CONSOLE_TITLE = "Intelligent Document Processing and Analysis API";

            // SQLITE_CONNECTION_STRING
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:SQLITE_CONNECTION_STRING"]))
                SQLITE_CONNECTION_STRING = configuration["AppSettings:SQLITE_CONNECTION_STRING"]!;
            else
                SQLITE_CONNECTION_STRING = "Data Source=Data/SQLite.db";

            // LLM_HTTP_API_IP
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:LLM_HTTP_API_IP"]))
                LLM_HTTP_API_IP = configuration["AppSettings:LLM_HTTP_API_IP"]!;
            else
                LLM_HTTP_API_IP = "localhost";

            // LLM_HTTP_API_PORT
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:LLM_HTTP_API_PORT"]))
                LLM_HTTP_API_PORT = configuration["AppSettings:LLM_HTTP_API_PORT"]!;
            else
                LLM_HTTP_API_PORT = "1234";

            // LLM_COMPLETION_ENDPOINT
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:LLM_COMPLETION_ENDPOINT"]))
                LLM_COMPLETION_ENDPOINT = configuration["AppSettings:LLM_COMPLETION_ENDPOINT"]!;
            else
                LLM_COMPLETION_ENDPOINT = "/v1/chat/completions";

            // HTTP_API_IP
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:HTTP_API_IP"]))
                HTTP_API_IP = IPAddress.TryParse(configuration["AppSettings:HTTP_API_IP"], out IPAddress? address) ? (address ?? IPAddress.Parse("127.0.0.1")) : IPAddress.Parse("127.0.0.1");
            else
                HTTP_API_IP = IPAddress.Parse("127.0.0.1");

            // HTTP_API_PORT
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:HTTP_API_PORT"]))
                HTTP_API_PORT = int.TryParse(configuration["AppSettings:HTTP_API_PORT"], out int result) ? result : 7015;
            else
                HTTP_API_PORT = 7015;

            // COMPLETION_HTTP_CLIENT_TIMEOUT
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:COMPLETION_HTTP_CLIENT_TIMEOUT"]))
                COMPLETION_HTTP_CLIENT_TIMEOUT = int.TryParse(configuration["AppSettings:COMPLETION_HTTP_CLIENT_TIMEOUT"], out int result) ? result : 180;
            else
                COMPLETION_HTTP_CLIENT_TIMEOUT = 180;

            // DISABLE_CONSOLE_QUICK_EDIT_MODE
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:DISABLE_CONSOLE_QUICK_EDIT_MODE"]))
                DISABLE_CONSOLE_QUICK_EDIT_MODE = !bool.TryParse(configuration["AppSettings:DISABLE_CONSOLE_QUICK_EDIT_MODE"], out bool result) || result;
            else
                DISABLE_CONSOLE_QUICK_EDIT_MODE = true;

            // ENABLE_VERBOSE_LOGS_TO_FILE
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:ENABLE_VERBOSE_LOGS_TO_FILE"]))
                ENABLE_VERBOSE_LOGS_TO_FILE = bool.TryParse(configuration["AppSettings:ENABLE_VERBOSE_LOGS_TO_FILE"], out bool result) && result;
            else
                ENABLE_VERBOSE_LOGS_TO_FILE = false;

            // ENABLE_DEBUG_LOGS_TO_FILE
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:ENABLE_DEBUG_LOGS_TO_FILE"]))
                ENABLE_DEBUG_LOGS_TO_FILE = bool.TryParse(configuration["AppSettings:ENABLE_DEBUG_LOGS_TO_FILE"], out bool result) && result;
            else
                ENABLE_DEBUG_LOGS_TO_FILE = false;

            // ENABLE_INFORMATION_LOGS_TO_FILE
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:ENABLE_INFORMATION_LOGS_TO_FILE"]))
                ENABLE_INFORMATION_LOGS_TO_FILE = bool.TryParse(configuration["AppSettings:ENABLE_INFORMATION_LOGS_TO_FILE"], out bool result) && result;
            else
                ENABLE_INFORMATION_LOGS_TO_FILE = false;

            // ENABLE_SWAGGER
            if (!string.IsNullOrWhiteSpace(configuration["AppSettings:ENABLE_SWAGGER"]))
                ENABLE_SWAGGER = !bool.TryParse(configuration["AppSettings:ENABLE_SWAGGER"], out bool result) || result;
            else
                ENABLE_SWAGGER = true;

            Log.Information("AppSettingsConstants loaded successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(AppSettingsConstants), nameof(AppSettingsConstants));

            throw;
        }
    }
}