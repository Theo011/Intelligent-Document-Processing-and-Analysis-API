using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionUsage
{
    [Required]
    [JsonPropertyName("prompt_tokens")]
    public long PromptTokens { get; set; }

    [Required]
    [JsonPropertyName("completion_tokens")]
    public long CompletionTokens { get; set; }

    [Required]
    [JsonPropertyName("total_tokens")]
    public long TotalTokens { get; set; }
}