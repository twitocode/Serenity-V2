using System.ComponentModel.DataAnnotations;
using Serenity.Common;
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

public class CreatePostResponse : Response
{

    public CreatePostResponse(bool success, List<ApplicationError> errors) : base(success, errors)
    {

    }

    public CreatePostResponse() { }
}