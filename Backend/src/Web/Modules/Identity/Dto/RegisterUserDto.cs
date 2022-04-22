using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Serenity.Modules.Identity.Dto;

public class RegisterUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(4)]
    public string Username { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    public string Avatar { get; set; }
    public string DiscordProfile { get; set; }
    public string InstagramProfile { get; set; }
    public string Bio { get; set; }
    public List<string> FollowedTags { get; set; }
}

public class RegisterUserResponse
{
    public bool Success { get; set; }
    public List<IdentityError> Errors { get; set; }
}