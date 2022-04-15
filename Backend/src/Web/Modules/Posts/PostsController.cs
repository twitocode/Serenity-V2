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
    public async Task<ActionResult<ICollection<Post>>> GetRecentPosts()
    {
        var posts = await postsService.GetRecentPostsAsync();
        return Ok(posts);
    }

    [HttpGet("my")]
    public async Task<ActionResult<ICollection<Post>>> GetUserPosts()
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var posts = await postsService.GetUserPostsAsync(user);

        return Ok(posts);
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<ICollection<Post>>> GetUserPostsById([FromRoute] string id)
    {
        var posts = await postsService.GetUserPostsByIdAsync(id);
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ICollection<Post>>> GetPostById([FromRoute] string id)
    {
        var posts = await postsService.GetPostByIdAsync(id);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeletePost([FromRoute] string id)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.DeletePostAsync(user, id);

        if (response)
            return Ok(response);
        else
            return BadRequest(response);
    }
}