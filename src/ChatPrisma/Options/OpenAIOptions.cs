using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public record OpenAIOptions
{
    public const string Section = "OpenAI";

    public string? Model { get; set; }

    public string? ApiKey { get; set; }
}
