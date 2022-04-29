using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Serenity.Database.Entities;

public class User : IdentityUser
{
    [Required]
    public string Avatar { get; set; }

    public string Bio { get; set; }

    public string DiscordProfile { get; set; }

    public string InstagramProfile { get; set; }

    public List<string> FollowedTags { get; set; }

    public List<Post> Posts { get; set; }
    public List<User> Friends { get; set; }
}