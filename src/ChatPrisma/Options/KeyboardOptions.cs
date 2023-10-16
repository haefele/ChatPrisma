using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options
{
    public class KeyboardOptions
    {
        [Required]
        public string Key { get; set; } = default!;
        [Required]
        public string KeyModifiers { get; set; } = default!;
    }
}
