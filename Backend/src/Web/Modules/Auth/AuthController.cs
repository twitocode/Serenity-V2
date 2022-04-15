
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Auth.Dto;

namespace Serenity.Modules.Auth;

[Route("auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly IHttpContextAccessor accessor;

    public AuthController(IAuthService authService, IHttpContextAccessor accessor)
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
    public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserDto dto)
    {
        var response = await authService.RegisterUserAsync(dto);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> LoginUser([FromBody] LoginUserDto dto)
    {
        var response = await authService.LoginAsync(dto);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser()
    {
        var user = await authService.GetUserAsync(HttpContext?.User);
        return Ok(user);
    }
}