using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionMessage(string role, string content)
{
    [Required]
    [JsonPropertyName("role")]
    [AllowedValues(Globals.CompletionMessageRoleSystem, Globals.CompletionMessageRoleUser, Globals.CompletionMessageRoleAssistant)]
    public string Role { get; set; } = role;

    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; } = content;
}