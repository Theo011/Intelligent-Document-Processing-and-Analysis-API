using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intelligent_Document_Processing_and_Analysis_API.Models.Entities;

public class LlmInteraction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    [Required]
    public string Input { get; private set; } = null!;

    [Required]
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    public string? Output { get; private set; }

    [StringLength(1000)]
    public string? Metadata { get; private set; }

    public LlmInteraction(string input, string? output = null, string? metadata = null)
    {
        Input = input;
        Output = output;
        Metadata = metadata;
    }

    private LlmInteraction() { }
}