using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

public class CompletionMessage
{
    [JsonPropertyName("role")]
    [AllowedValues(Globals.CompletionMessageRoleSystem, Globals.CompletionMessageRoleUser, Globals.CompletionMessageRoleAssistant)]
    public required string Role { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }
}