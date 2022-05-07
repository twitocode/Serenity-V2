using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Modules.Identity.Dto;
using Serenity.Modules.Identity.Handlers;

namespace Serenity.Modules.Identity;

[Route("auth")]
[AllowAnonymous]
public class IdentityController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        => ResultHandler.Handle(await mediator.Send(new RegisterUserCommand(dto)));

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
        => ResultHandler.Handle(await mediator.Send(new LoginUserCommand(dto)));

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetUser()
        => Ok(await mediator.Send(new GetUserQuery(HttpContext?.User)));
}