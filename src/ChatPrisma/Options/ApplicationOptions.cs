﻿using System.ComponentModel.DataAnnotations;

namespace ChatPrisma.Options;

public record ApplicationOptions
{
    [Required]
    public string ApplicationName { get; set; } = default!;
    [Required]
    public string ApplicationVersion { get; set; } = default!;
    [Required]
    public string CommitId { get; set; } = default!;
    public bool IsPublicVersion { get; set; } = false;
    [Required]
    public string ContactName { get; set; } = default!;
    [Required]
    public string ContactEmailAddress { get; set; } = default!;
    [Required]
    public string GitHubLink { get; set; } = default!;
}
