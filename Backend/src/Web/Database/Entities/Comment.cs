using System.ComponentModel.DataAnnotations;

namespace Serenity.Database.Entities;

public class Comment
{
    [Key]
    public string Id { get; set; }
    public string Content { get; set; }

    public string PostId { get; set; }
    public string UserId { get; set; }
    public string RepliesToId { get; set;}

    public Post Post { get; set; }
    public User User { get; set; }
    public Comment RepliesTo { get; set; }
}
