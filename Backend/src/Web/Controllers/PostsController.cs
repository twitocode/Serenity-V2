using Application.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly AuthHandler authHandler;
    private readonly PostsHandler postsHandler;

    public PostsController(AuthHandler authHandler, PostsHandler postsHandler)
    {
        this.authHandler = authHandler;
        this.postsHandler = postsHandler;
    }

    [HttpGet]
    public async void GetUserPosts()
    {
        var user = await authHandler.GetUserAsync(HttpContext.User);
        
    }
}