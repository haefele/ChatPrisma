using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public class OpenAIOptions
{
    [Required]
    public string Model { get; set; } = default!;

    [Required]
    public string ApiKey { get; set; } = default!;
}
