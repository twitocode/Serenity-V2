using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Common.Interfaces;
using Serenity.Modules.Comments.Dto;
using Serenity.Modules.Comments.Handlers;

namespace Serenity.Modules.Comments;

[Route("posts/{postId}")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator mediator;

    public CommentsController(IMediator mediator) => this.mediator = mediator;

    [HttpGet("all")]
    public async Task<IActionResult> GetComments([FromRoute] string postId)
        => Ok(await mediator.Send(new GetCommentsQuery(postId)));

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] string postId, [FromBody] CreateCommentDto dto)
        => ResultHandler.Handle(await mediator.Send(new CreateCommentCommand(dto, HttpContext.User, postId)));

    [HttpPost("{commentId}")]
    public async Task<IActionResult> Reply([FromRoute] string postId, [FromBody] CreateCommentDto dto)
        => ResultHandler.Handle(await mediator.Send(new ReplyToCommentCommand(dto, HttpContext.User, postId)));

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> Delete([FromRoute] string postId, [FromRoute] string commentId)
        => ResultHandler.Handle(await mediator.Send(new DeleteCommentCommand(commentId, HttpContext.User, postId)));

    [HttpPut]
    public async Task<IActionResult> Edit([FromRoute] string postId, [FromBody] EditCommentDto dto)
        => ResultHandler.Handle(await mediator.Send(new EditCommentCommand(dto, HttpContext.User, postId)));
}