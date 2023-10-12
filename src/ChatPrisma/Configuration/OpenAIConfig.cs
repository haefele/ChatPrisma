namespace ChatPrisma.Configuration;

public class OpenAIConfig
{
    public string Model { get; set; } = "gpt-3.5-turbo";
    public string? ApiKey { get; set; }
}