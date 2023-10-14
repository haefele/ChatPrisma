using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public class ApplicationOptions
{
    [Required]
    public string ApplicationName { get; set; } = default!;
    [Required]
    public string ApplicationVersion { get; set; } = default!;
    [Required]
    public string ContactName { get; set; } = default!;
    [Required]
    public string ContactEmailAddress { get; set; } = default!;
}