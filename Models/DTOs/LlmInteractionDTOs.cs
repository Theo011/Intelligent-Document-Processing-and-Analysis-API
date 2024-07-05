using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.DTOs;

public class CreateLlmInteractionDto
{
    public required string Input { get; set; }
    public string? Output { get; set; }
    public string? Metadata { get; set; }

    public override string ToString()
    {
        try
        {
            return $"Input: {Input}, Output: {Output}, Metadata: {Metadata}";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(CreateLlmInteractionDto), nameof(ToString));

            return string.Empty;
        }
    }
}

public class LlmInteractionDto
{
    public int Id { get; set; }
    public string Input { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public string? Output { get; set; }
    public string? Metadata { get; set; }
}