namespace Intelligent_Document_Processing_and_Analysis_API.Utilities;

public static class Globals
{
    public const string CompletionMessageRoleSystem = "system";
    public const string CompletionMessageRoleUser = "user";
    public const string CompletionMessageRoleAssistant = "assistant";

    public const string CompletionHttpClientName = "CompletionHttpClient";

    public const string CompletionRequestsFolderName = "CompletionRequests";
    public const string LogsFolderName = "LOGS";
    public const string UploadsFolderName = "Uploads";

    public const float DefaultCompletionTemperature = 0.3f;

    public const string PdfDocumentExtension = ".pdf";
    public const string DocDocumentExtension = ".doc";
    public const string DocxDocumentExtension = ".docx";
    public const string TxtDocumentExtension = ".txt";
    public readonly static string[] allowedDocumentExtensions = [PdfDocumentExtension, DocDocumentExtension, DocxDocumentExtension, TxtDocumentExtension];
    public readonly static string allowedDocumentExtensionsString = string.Join(", ", allowedDocumentExtensions);

    /*public readonly static JsonSerializerOptions jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };*/

    static Globals()
    {
        /*try
        {
            // allowedDocumentExtensionsString = string.Join(", ", allowedDocumentExtensions);

            Log.Information("Globals loaded successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(Globals), nameof(Globals));

            throw;
        }*/
    }
}