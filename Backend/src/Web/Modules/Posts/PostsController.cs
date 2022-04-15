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
        var posts = await postsService.GetUserPostsAsync(user);

        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult<CreatePostResponse>> CreatePost([FromBody] CreatePostDto dto)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.CreatePostAsync(user, dto);

        if (response.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeletePost([FromQuery] string postId)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.DeletePostAsync(user, postId);

        if (response)
            return Ok(response);
        else
            return BadRequest(response);
    }
}