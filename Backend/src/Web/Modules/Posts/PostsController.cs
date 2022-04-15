using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts;

[Authorize]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly IPostsService postsService;

    public PostsController(IAuthService authService, IPostsService postsService)
    {
        this.authService = authService;
        this.postsService = postsService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Post>>> GetUserPosts()
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        return Ok(user.Posts);
    }

    [HttpPost]
    public async Task<ActionResult<CreatePostResponse>> CreatePost(CreatePostDto dto)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.CreatePostAsync(user, dto);

        if (response.Success)
            return Ok(response);

        else
            return BadRequest(response);
    }
}