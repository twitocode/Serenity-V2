using System.ComponentModel.DataAnnotations;

namespace Serenity.Database.Entities;

public class Post
{
    [Key]
    public Guid Id;
    public string CreatedAt { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }
    public List<string> Tags { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public List<Comment> Comments { get; set; }
}
