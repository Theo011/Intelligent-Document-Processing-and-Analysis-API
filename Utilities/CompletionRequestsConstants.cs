using Intelligent_Document_Processing_and_Analysis_API.Models.LLM;
using Newtonsoft.Json;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Utilities;

public static class CompletionRequestsConstants
{
    public readonly static CompletionRequest TestCompletionRequest = null!;

    static CompletionRequestsConstants()
    {
        try
        {
            // TestCompletionRequest
            string testCompletionRequestJsonFilePath = Path.Combine(Globals.CompletionRequestsFolderName, "TestCompletionRequest.json");
            string testCompletionRequestJsonContent = File.ReadAllText(testCompletionRequestJsonFilePath);

            TestCompletionRequest = JsonConvert.DeserializeObject<CompletionRequest>(testCompletionRequestJsonContent)
                ?? throw new($"Failed to deserialize completion request at class: {nameof(CompletionRequestsConstants)}, method: {nameof(CompletionRequestsConstants)} with input: {testCompletionRequestJsonContent}");

            Log.Information("CompletionRequestsConstants loaded successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(CompletionRequestsConstants), nameof(CompletionRequestsConstants));

            throw;
        }
    }
}