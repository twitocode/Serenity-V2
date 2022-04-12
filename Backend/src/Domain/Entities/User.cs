using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string Bio { get; set; }
        public string Avatar { get; set; }
        public string DiscordProfile { get; set; }
        public string InstagramProfile { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<User> Friends { get; set; }
    }
}