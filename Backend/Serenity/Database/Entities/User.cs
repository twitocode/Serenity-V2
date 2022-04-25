using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Serenity.Database.Entities;

public class User : IdentityUser
{
    [Required]
    public string Avatar { get; set; }

    [Required]
    public string Bio { get; set; }

    [Required]
    public string DiscordProfile { get; set; }

    [Required]
    public string InstagramProfile { get; set; }

    [Required]
    public List<string> FollowedTags { get; set; }

    public List<Post> Posts { get; set; }
    public List<User> Friends { get; set; }
}
