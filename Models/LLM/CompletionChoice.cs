using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionChoice
{
    [Required]
    [JsonPropertyName("index")]
    public long Index { get; set; }

    [Required]
    [JsonPropertyName("message")]
    public CompletionMessage Message { get; set; } = null!;

    [Required]
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = null!;
}