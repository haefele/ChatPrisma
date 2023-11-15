namespace ChatPrisma.Options;

public record TextEnhancementOptions
{
    public const string Section = "TextEnhancement";

    public int TextSize { get; set; }
    public string? CustomInstructions { get; set; }
}
