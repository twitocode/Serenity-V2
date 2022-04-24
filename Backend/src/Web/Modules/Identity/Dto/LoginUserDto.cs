using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Common.Errors;

namespace Serenity.Modules.Identity.Dto;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginUserResponse : Response
{
    public string Token { get; set; }

    public LoginUserResponse(bool success, List<ApplicationError> errors, string token) : base(success, errors)
    {
        Token = token;
    }

    public LoginUserResponse() {}
}