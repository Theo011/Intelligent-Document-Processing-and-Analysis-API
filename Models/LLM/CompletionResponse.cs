using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionResponse
{
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [Required]
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;

    [Required]
    [JsonPropertyName("created")]
    public long Created { get; set; }

    [Required]
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;

    [Required]
    [JsonPropertyName("choices")]
    public List<CompletionChoice> Choices { get; set; } = null!;

    [Required]
    [JsonPropertyName("usage")]
    public CompletionUsage Usage { get; set; } = null!;
}