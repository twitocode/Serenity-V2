using System.ComponentModel.DataAnnotations;
using Serenity.Common.Errors;

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

public class CreatePostResponse
{
    public bool Success { get; set; }
    public List<ApplicationError> Errors { get; set; }
}