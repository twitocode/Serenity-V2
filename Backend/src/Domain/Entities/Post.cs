using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
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

        [Required]
        public string UserId { get; set; }

        [Required]
        public User User { get; set; }

        public List<Comment> Comments { get; set; }

    }
}