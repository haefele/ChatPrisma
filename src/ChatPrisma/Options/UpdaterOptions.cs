using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public class UpdaterOptions
{
    [Required]
    public string GitHubUsername { get; set; } = default!;
    [Required]
    public string GitHubRepository { get; set; } = default!;
    [Required]
    public string GitHubReleaseAssetName { get; set; } = default!;

    public bool CheckForUpdatesInBackground { get; set; }
    public int MinutesBetweenUpdateChecks { get; set; }
}
