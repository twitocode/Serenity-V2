using System.ComponentModel.DataAnnotations;

namespace Serenity.Database.Entities;

public class Comment
{
    [Key]
    public Guid Id;
    public string Content { get; set; }

    [Required]
    public Guid PostId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public Post Post { get; set; }

    [Required]
    public User User { get; set; }
}
