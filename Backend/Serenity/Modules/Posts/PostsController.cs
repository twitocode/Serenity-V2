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

    [HttpGet("tag/{tag}")]
    public async Task<IActionResult> GetPostsByTag([FromQuery] int page, [FromRoute] string tag)
        => ResultHandler.Handle(await mediator.Send(new GetPostsByTagQuery(page, tag)));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new GetPostByIdQuery(id)));

    [HttpGet("user")]
    public async Task<IActionResult> GetUserPosts([FromQuery] int page)
        => ResultHandler.Handle(await mediator.Send(new GetMyPostsQuery(HttpContext?.User, page)));

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserPostsById([FromRoute] string id, [FromQuery] int page)
        => ResultHandler.Handle(await mediator.Send(new GetUserPostsByIdQuery(id, page)));

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        => ResultHandler.Handle(await mediator.Send(new CreatePostCommand(dto, HttpContext.User)));

    [HttpPut("{id}")]
    public async Task<IActionResult> EditPost([FromBody] EditPostDto dto, [FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new EditPostCommand(dto, HttpContext.User, id)));

    [HttpPut("{id}/tags/{tag}")]
    public async Task<IActionResult> AddTagToPost([FromRoute] string tag, [FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new AddTagCommand(HttpContext.User, id, tag)));

    [HttpDelete("{id}/tags/{tag}")]
    public async Task<IActionResult> RemoveTagToPost([FromRoute] string tag, [FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new RemoveTagCommand(HttpContext.User, id, tag)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new DeletePostCommand(id, HttpContext.User)));
}