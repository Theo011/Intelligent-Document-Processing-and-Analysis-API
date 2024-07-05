using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionUsage
{
    [JsonPropertyName("prompt_tokens")]
    public required long PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public required long CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public required long TotalTokens { get; set; }
}