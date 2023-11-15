using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public record OpenAIOptions
{
    public const string Section = "OpenAI";

    [Required]
    public string Model { get; set; } = default!;

    [Required]
    public string ApiKey { get; set; } = default!;
}
