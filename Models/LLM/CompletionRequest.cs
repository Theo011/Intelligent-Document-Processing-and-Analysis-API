using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

/*
 * https://platform.openai.com/docs/api-reference/chat/create
 * Supported payload parameters for LM Studio's completion (as of 05-July-2024) :
 *  model
 *  top_p
 *  top_k
 *  messages
 *  temperature
 *  max_tokens
 *  stream
 *  stop
 *  presence_penalty
 *  frequency_penalty
 *  logit_bias
 *  repeat_penalty
 *  seed
 */

public class CompletionRequest(List<CompletionMessage> messages, float temperature = Globals.DefaultCompletionTemperature, bool stream = false)
{
    [Required]
    [JsonPropertyName("messages")]
    public List<CompletionMessage> Messages { get; set; } = messages;

    [Required]
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = temperature;

    [Required]
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = stream;
}