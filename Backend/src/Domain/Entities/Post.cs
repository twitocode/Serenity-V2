using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Post
    {
        [Key]
        public Guid Id;
        public string CreatedAt { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}