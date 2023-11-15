using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public record HotkeyOptions
{
    public const string Section = "Hotkey";

    [Required]
    public string Key { get; set; } = default!;
    [Required]
    public string KeyModifiers { get; set; } = default!;
    public int HotkeyDelayInMilliseconds { get; set; }
    public int ClipboardDelayInMilliseconds { get; set; }
}
