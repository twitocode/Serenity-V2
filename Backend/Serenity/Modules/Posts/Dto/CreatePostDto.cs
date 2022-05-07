using System.ComponentModel.DataAnnotations;

namespace Serenity.Modules.Posts.Dto;

public class CreatePostDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public List<string> Tags { get; set; }
}