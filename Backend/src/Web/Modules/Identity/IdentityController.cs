
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity;

[Route("auth")]
[AllowAnonymous]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService authService;
    private readonly IHttpContextAccessor accessor;

    public IdentityController(IIdentityService authService, IHttpContextAccessor accessor)
    {
        this.authService = authService;
        this.accessor = accessor;
    }

    [HttpGet]
    public string HelloWorld()
    {
        return "Hello world";
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
    {
        var response = await authService.RegisterUserAsync(dto);
        return ResultHandler.Handle(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
    {
        var response = await authService.LoginAsync(dto);
        return ResultHandler.Handle(response);
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var user = await authService.GetUserAsync(HttpContext?.User);
        return Ok(user);
    }
}