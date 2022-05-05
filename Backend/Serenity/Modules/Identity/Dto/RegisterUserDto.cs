using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Common.Errors;

namespace Serenity.Modules.Identity.Dto;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Avatar { get; set; }
    public string DiscordProfile { get; set; }
    public string InstagramProfile { get; set; }
    public string Bio { get; set; }
    public List<string> FollowedTags { get; set; }
}

public class RegisterUserResponse : Response
{
    public RegisterUserResponse(bool success, List<ApplicationError> errors, string token) : base(success, errors)
    {

    }

    public RegisterUserResponse() { }
}