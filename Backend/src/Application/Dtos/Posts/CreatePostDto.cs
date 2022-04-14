using System.ComponentModel.DataAnnotations;
using Domain.Errors;

namespace Application.Dtos.Posts;

public class CreatePostDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class CreatePostResponse
{
    public string Token { get; set; }
    public bool Success { get; set; }
    public List<ApplicationError> Errors { get; set; }
}