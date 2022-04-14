using Application.Dtos.Auth;
using Application.Handlers;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AuthHandler authHandler;
    private readonly IHttpContextAccessor accessor;

    public AuthController(AuthHandler authHandler, IHttpContextAccessor accessor)
    {
        this.authHandler = authHandler;
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
        var response = await authHandler.RegisterUserAsync(dto);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> LoginUser([FromBody] LoginUserDto dto)
    {
        var response = await authHandler.LoginAsync(dto);

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
        var user = await authHandler.GetUserAsync(accessor.HttpContext?.User);
        return Ok(user);
    }
}