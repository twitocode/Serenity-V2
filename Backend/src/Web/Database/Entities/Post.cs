using System.ComponentModel.DataAnnotations;

namespace Serenity.Database.Entities;

public class Post
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public List<string> Tags { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public List<Comment> Comments { get; set; }
}
