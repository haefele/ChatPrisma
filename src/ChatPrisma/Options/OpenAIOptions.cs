namespace ChatPrisma.Options;

public class OpenAIOptions
{
    public string Model { get; set; } = "gpt-3.5-turbo";
    public string? ApiKey { get; set; }
}