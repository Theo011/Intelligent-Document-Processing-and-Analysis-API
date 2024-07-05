using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("created")]
    public required long Created { get; set; }

    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("choices")]
    public required List<CompletionChoice> Choices { get; set; }

    [JsonPropertyName("usage")]
    public required CompletionUsage Usage { get; set; }
}