using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts;

[Authorize]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IIdentityService authService;
    private readonly IPostsService postsService;

    public PostsController(IIdentityService authService, IPostsService postsService)
    {
        this.authService = authService;
        this.postsService = postsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecentPosts()
    {
        var posts = await postsService.GetRecentPostsAsync();
        return Ok(posts);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetUserPosts()
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var posts = await postsService.GetUserPostsAsync(user);

        return Ok(posts);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserPostsById([FromRoute] string id)
    {
        var posts = await postsService.GetUserPostsByIdAsync(id);
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] string id)
    {
        var posts = await postsService.GetPostByIdAsync(id);
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.CreatePostAsync(user, dto);

        return ResultHandler.Handle(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditPost([FromBody] EditPostDto dto, [FromRoute] string id)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.EditPostAsync(user, id, dto);

        return ResultHandler.Handle(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] string id)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var response = await postsService.DeletePostAsync(user, id);

        return ResultHandler.Handle(response);
    }
}