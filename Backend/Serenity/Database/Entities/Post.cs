using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace Serenity.Database.Entities;

public class Post
{
    [Key]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Instant CreationTime { get; set; }

    [NotMapped]
    public string CreationTimeString { get => CreationTime.ToDateTimeUtc().ToString(); }
    public List<string> Tags { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public List<Comment> Comments { get; set; }
}
