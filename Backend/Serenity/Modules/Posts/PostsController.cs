using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Modules.Posts.Dto;
using Serenity.Modules.Posts.Handlers;

namespace Serenity.Modules.Posts;

[Authorize]
[Route("posts")]
public class PostsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRecentPosts([FromQuery] int page)
        => ResultHandler.Handle(await mediator.Send(new GetRecentPostsQuery(page)));

    [HttpGet("my")]
    public async Task<IActionResult> GetUserPosts([FromQuery] int page)
        => ResultHandler.Handle(await mediator.Send(new GetMyPostsQuery(HttpContext?.User, page)));

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserPostsById([FromRoute] string id, [FromQuery] int page)
        => ResultHandler.Handle(await mediator.Send(new GetUserPostsByIdQuery(id, page)));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new GetPostByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        => ResultHandler.Handle(await mediator.Send(new CreatePostCommand(dto, HttpContext.User)));

    [HttpPut("{id}")]
    public async Task<IActionResult> EditPost([FromBody] EditPostDto dto, [FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new EditPostCommand(dto, HttpContext.User, id)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new DeletePostCommand(id, HttpContext.User)));
}