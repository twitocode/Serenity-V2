using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id;
        public string Content { get; set; }

        public Guid PostId { get; set; }
        public string UserId { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
    }
}