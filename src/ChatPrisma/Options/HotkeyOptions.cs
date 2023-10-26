using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options
{
    public class HotkeyOptions
    {
        [Required]
        public string Key { get; set; } = default!;
        [Required]
        public string KeyModifiers { get; set; } = default!;
        public int HotkeyDelayInMilliseconds { get; set; }
        public int ClipboardDelayInMilliseconds { get; set; }
    }
}
