using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionChoice
{
    [JsonPropertyName("index")]
    public required long Index { get; set; }

    [JsonPropertyName("message")]
    public required CompletionMessage Message { get; set; }

    [JsonPropertyName("finish_reason")]
    public required string FinishReason { get; set; }
}