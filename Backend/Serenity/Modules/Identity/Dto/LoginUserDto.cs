using System.ComponentModel.DataAnnotations;

namespace Serenity.Modules.Identity.Dto;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}